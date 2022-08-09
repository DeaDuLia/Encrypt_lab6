using System.Drawing;
using System.Text;
using System.IO;

namespace lab_6
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap myBitmap = (Bitmap)Image.FromFile("C:\\Users\\1\\Desktop\\lab_7\\cat.jpg");
            string message = "I use this text for test my encryption:)";
            List<ulong> blocks = getBlocksFromMessage(message);
            Console.WriteLine($"Your Message: { message }");
            encrypt(blocks, myBitmap);
            Console.WriteLine($"Message encrypted");
            string newFileName = "C:\\Users\\1\\Desktop\\lab_7\\encrypted_cat.jpg";
            createFile(myBitmap, newFileName);
            // Создать файл
        }

        private static void createFile(Bitmap myBitmap, string fileName)
        {
            using (FileStream fstream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                myBitmap.Save(fstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                Console.WriteLine("File was Created");
            }
        }
        private static List<ulong> getBlocksFromMessage(string msg)
        {
            List<ulong> res = new List<ulong>();

            for (int i = 0; i < msg.Length % 8; i++)
            {
                msg = msg + 0;
            }
            byte[] str = Encoding.Default.GetBytes(msg);
            for (int i = 0; i < str.Length; i += 8)
            {
                byte[] tmp = new byte[8];
                tmp[0] = str[i];
                tmp[1] = str[i + 1];
                tmp[2] = str[i + 2];
                tmp[3] = str[i + 3];
                tmp[4] = str[i + 4];
                tmp[5] = str[i + 5];
                tmp[6] = str[i + 6];
                tmp[7] = str[i + 7];
                res.Add(BitConverter.ToUInt64(tmp, 0));
            }
            return res;
        }

        private static void encrypt(List<ulong> blocks, Bitmap bitmap)
        {
            //if (blocks.Count * 8 > ) throw new Exception("Не хватает ех...");
            int tmp = 0;
            int tmp2 = 0;
            bool flag = false;
            bool flag2 = false;
            for (int i = 0, v = 0, h = 0; i < blocks.Count && tmp2 < blocks.Count; i = flag2 ? tmp2 : (i + 1), flag2 = false, tmp2 = 0)
            {
                for (int j = 0; j < 64 && tmp < 64 && tmp2 < blocks.Count; j = flag ? tmp : (j+1), flag = false, tmp = 0)
                {
                    int bit = getBit(blocks[i], j);
                    Color color = bitmap.GetPixel(h, v);
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;
                    R = ((R >> 2) << 2) | (bit << 1);
                    if (j < 63)
                    {
                        j++;
                        bit = getBit(blocks[i], j);
                        R = ((R >> 1) << 1) | bit;
                        if (j < 63)
                        {
                            j++;
                            bit = getBit(blocks[i], j);
                            G = ((G >> 2) << 2) | (bit << 1);
                            if (j < 63)
                            {
                                j++;
                                bit = getBit(blocks[i], j);
                                G = ((G >> 1) << 1) | bit;
                                if (j < 63)
                                {
                                    j++;
                                    bit = getBit(blocks[i], j);
                                    B = ((B >> 2) << 2) | (bit << 1);
                                    if (j < 63)
                                    {
                                        j++;
                                        bit = getBit(blocks[i], j);
                                        B = ((B >> 1) << 1) | bit;
                                    } else
                                    {
                                        i++;
                                        if (i < blocks.Count)
                                        {
                                            j = 0;
                                            bit = getBit(blocks[i], j);
                                            B = ((B >> 1) << 1) | bit;
                                            
                                        }
                                        tmp = j;
                                        tmp2 = i;
                                        flag = true;
                                        flag2 = true;
                                    }
                                } else
                                {
                                    i++;
                                    if (i < blocks.Count)
                                    {
                                        j = 0;
                                        bit = getBit(blocks[i], j);
                                        B = ((B >> 2) << 2) | (bit << 1);

                                        j++;
                                        bit = getBit(blocks[i], j);
                                        B = ((B >> 1) << 1) | bit;
                                        
                                    }
                                    tmp = j;
                                    tmp2 = i;
                                    flag = true;
                                    flag2 = true;
                                }
                            }
                        } else
                        {
                            i++;
                            if (i < blocks.Count)
                            {
                                j = 0;
                                bit = getBit(blocks[i], j);
                                G = ((G >> 2) << 2) | (bit << 1);

                                j++;
                                bit = getBit(blocks[i], j);
                                G = ((G >> 1) << 1) | bit;

                                j++;
                                bit = getBit(blocks[i], j);
                                B = ((B >> 2) << 2) | (bit << 1);

                                j++;
                                bit = getBit(blocks[i], j);
                                B = ((B >> 1) << 1) | bit;
                                
                            }
                            tmp = j;
                            tmp2 = i;
                            flag = true;
                            flag2 = true;
                        }
                    } else
                    {
                        i++;
                        if (i < blocks.Count)
                        {
                            j = 0;
                            bit = getBit(blocks[i], j);
                            R = ((R >> 1) << 1) | bit;

                            j++;
                            bit = getBit(blocks[i], j);
                            G = ((G >> 2) << 2) | (bit << 1);

                            j++;
                            bit = getBit(blocks[i], j);
                            G = ((G >> 1) << 1) | bit;

                            j++;
                            bit = getBit(blocks[i], j);
                            B = ((B >> 2) << 2) | (bit << 1);

                            j++;
                            bit = getBit(blocks[i], j);
                            B = ((B >> 1) << 1) | bit;
                        }
                        tmp = j;
                        tmp2 = i;
                        flag = true;
                        flag2 = true;

                    }
                    bitmap.SetPixel(h, v, Color.FromArgb(color.A, R, G, B));
                    if (h >= bitmap.HorizontalResolution)
                    {
                        v++;
                        h = 0;
                    } else
                    {
                        h++;
                    }
                }
            }
        }

        private static int getBit(ulong val, int ind)
        {
            int bit = 0;
            bit = (int)((val << ind) >> 63);
            return bit;
        }


    }
}