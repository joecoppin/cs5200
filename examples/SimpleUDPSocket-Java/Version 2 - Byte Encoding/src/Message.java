import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.Date;

public class Message {
    public static int Id = 1;
    public static Date Timestamp;
    public static String Text;

    public Message() { }

    public Message(String text) {
        Message.Id++;
        Message.Timestamp = new Date();
        Message.Text = text;
    }

    public static Message decode(byte[] bytes) {
        Message msg = new Message();
        ByteBuffer buff = ByteBuffer.wrap(bytes);
        buff.order(ByteOrder.BIG_ENDIAN);

        // get Id and timestamp
        msg.Id = buff.getInt();
        msg.Timestamp = new Date(buff.getLong());

        // get message
        short textLength = buff.getShort();
        byte[] textBytes = new byte[textLength];
        buff.get(textBytes, 0, textLength);
        msg.Text = new String(textBytes);

        return msg;
    }

    public static byte[] encode() throws IOException {
        // create a buffer to hold
        ByteArrayOutputStream baos = new ByteArrayOutputStream();

        // convert integer ID to bytes
        ByteBuffer idBuff = ByteBuffer.allocate(Integer.BYTES);
        // follow the Big Endian byte order
        idBuff.order(ByteOrder.BIG_ENDIAN);
        idBuff.putInt(Id);
        baos.write(idBuff.array());

        // convert long timestamp to bytes
        ByteBuffer timeBuff = ByteBuffer.allocate(Long.BYTES);
        timeBuff.order(ByteOrder.BIG_ENDIAN);
        timeBuff.putLong(Timestamp.getTime());
        baos.write(timeBuff.array());

        // put text in
        byte[] textBytes = Text.getBytes();
        idBuff = ByteBuffer.allocate(Short.BYTES);
        idBuff.order(ByteOrder.BIG_ENDIAN);
        idBuff.putShort((short) textBytes.length);
        baos.write(idBuff.array());

        baos.write(textBytes);

        return baos.toByteArray();
    }
}
