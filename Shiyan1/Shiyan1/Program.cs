using Gma.QrCodeNet.Encoding;
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
                case 1:
                    Console.WriteLine(EnCode('M', args[0]));
                    break;
                default:
                    Console.WriteLine("错误");
                    return;
            }
        }

        static String EnCode(char level, String content)
        {
            QrEncoder qrEncoder;
            String ret = "";
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
            QrCode qrCode = qrEncoder.Encode(content);
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
