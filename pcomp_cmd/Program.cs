using System;
using System.Text;
using System.IO;
using System.Net;
using pcomp;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace pcomp_cmd
{
    class PComp
    {
        static void Main(string[] args)
        {
            try
            {
                // 입력 매개변수 2개 여부 Check
                if (args.Length != 2)
                {
                    Console.WriteLine("2개의 매개변수만 사용이 가능합니다.(현재 {0}개)", args.Length);
                    Console.WriteLine("예시) pcomp.exe file1 file2 <ENTER>");
                    return;
                }
                else
                {
                    if (string.IsNullOrEmpty(args[0]))
                    {
                        Console.WriteLine("1번째 매개변수를 확인하세요.");
                        return;
                    }
                    if (string.IsNullOrEmpty(args[1]))
                    {
                        Console.WriteLine("2번째 매개변수를 확인하세요.");
                        return;
                    }
                }


                // 파일비교 객체 생성
                pcomp.PCompare compare = new pcomp.PCompare();

                // 두 파일 비교
                if (false == compare.Compare(args[0], args[1])) { return; }

            }
            catch (Exception e)
            {
                Console.WriteLine("예외 발생:{0}", e.Message);
                return;
            }
            return;
        }

    }
}
