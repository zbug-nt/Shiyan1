using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiyan1
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args.Length)
            {
                case 1: //唯一指定参数个数
                    if (args[0].Length >= 2 && args[0].Substring(0, 2).Equals("-f"))
                    {
                        //输入参数为文件名
                        if (args[0].Length == 2) goto default;
                        String[] lines;
                        try
                        {
                            lines = File.ReadAllLines(args[0].Substring(2));
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("无法打开指定文件，请检查文件名是否正确");
                            Console.ResetColor();
                            goto default;
                        }
                        String[] pathElements = args[0].Substring(2).Split('\\');
                        String path = "";
                        for (int i = 0; i < pathElements.Length - 1; ++i)
                        {
                            path += pathElements[i] + '\\';
                        }
                        GraphicsRenderer renderer = new GraphicsRenderer(new FixedCodeSize(256, QuietZoneModules.Four));
                        for (int i = 0; i < lines.Length; ++i)
                        {
                            Console.WriteLine("正在编码第" + (i + 1) + "行");
                            QrCode qrCode = EnCode('M', lines[i]);
                            if (qrCode == null) continue;
                            Console.WriteLine("编码成功，正在保存图片文件");
                            FileStream stream = new FileStream(path + i / 100 % 10 + i / 10 % 10 + i % 10 + lines[i].Substring(0, 4) + ".png", FileMode.Create);
                            renderer.WriteToStream(qrCode.Matrix, System.Drawing.Imaging.ImageFormat.Png, stream);
                            Console.WriteLine("保存成功");
                        }
                        Console.WriteLine("完成");
                    }
                    else
                    {
                        //输入参数为待编码信息
                        QrCode qrCode = EnCode('M', args[0]);
                        if (qrCode == null) goto default;
                        else Console.WriteLine(CodeToString(qrCode));
                    }
                    break;
                default: //出现问题，输出帮助信息
                    ShowHelpInfo();
                    return;
            }
        }
        //输出帮助信息
        static void ShowHelpInfo()
        {
            Console.WriteLine("参数格式说明：");
            Console.WriteLine("Shiyan1.exe [message] ：在控制台输出message的二维码");
            Console.WriteLine("Shiyan1.exe -f[path] ：为path指定的文件中的每行信息生成二维码并保存为png格式图片");
            Console.WriteLine("可编码的最大长度就随随便便定为100好了");
        }
        //编码QRCode
        static QrCode EnCode(char level, String content)
        {
            if (content.Length > 100)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("待编码信息超过可编码的最大长度");
                Console.ResetColor();
                return null;
            }
            QrEncoder qrEncoder;
            switch (level)
            {
                case 'L':
                    qrEncoder = new QrEncoder(ErrorCorrectionLevel.L);
                    break;
                case 'M':
                    qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
                    break;
                case 'Q':
                    qrEncoder = new QrEncoder(ErrorCorrectionLevel.Q);
                    break;
                case 'H':
                    qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                    break;
                default:
                    goto case 'M';
            }
            return qrEncoder.Encode(content);
        }
        //生成供控制台输出的字符串
        static String CodeToString(QrCode qrCode)
        {
            if (qrCode == null)
            {
                return "";
            }
            String ret = "";
            for (int i = -1; i <= qrCode.Matrix.Width; ++i) ret = ret + "█";
            ret = ret + "\n";
            for (int i = 0; i < qrCode.Matrix.Height; ++i)
            {
                ret = ret + "█";
                for (int j = 0; j < qrCode.Matrix.Width; ++j)
                    ret = ret + (qrCode.Matrix[i, j] ? "  " : "█");
                ret = ret + "█\n";
            }
            for (int i = -1; i <= qrCode.Matrix.Width; ++i) ret = ret + "█";
            return ret;
        }
    }
}
