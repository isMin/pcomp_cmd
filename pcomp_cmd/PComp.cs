using System;
using System.Text;
using System.IO;


namespace pcomp
{
    class PFile
    {
        /// <summary>
        /// 파일정보 (FileInfo의 객체).
        /// </summary>
        public FileInfo fi;
        public StreamReader srFile;

        public bool Check(string FullName)
        {
            try
            {
                Console.WriteLine("Check(string FullName)");
                fi = new FileInfo(FullName);

Console.WriteLine("fi.FullName: ({0})\nfi.Directory: ({1})\nfi.Name: ({2})\nfi.Extension: ({3})\nfi.DirectoryName: ({4})\nfi.Length: ({5})\n", fi.FullName, fi.Directory, fi.Name, fi.Extension, fi.DirectoryName, fi.Length);

                // 디렉토리 존재여부 Check
                if (!fi.Directory.Exists)
                {
                    Console.WriteLine("해당 디렉토리가 존재하지 않습니다.({0})", FullName);
                    return false;
                }

                // 파일 존재여부 Check
                if (!fi.Exists)
                {
                    Console.WriteLine("해당 파일이 존재하지 않습니다.({0})", FullName);
                    return false;
                }

                // 파일을 읽어 Buffer에 저장
                // 한글 인코딩 문제로 "euc-kr"을 Default 지정.
                srFile = new StreamReader(fi.FullName, Encoding.GetEncoding("euc-kr"));

                // Buffer null Check
                if (srFile == null)
                {
                    Console.WriteLine("[{0}]은(는) 비어있는 파일입니다.\n", fi.FullName);
                    srFile.Close();
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("예외 발생:{0}", e.Message);
                return false;
            }
            return true;
        }

    }

    class PCompare
    {
        //private string CompareResult;

        public PCompare(string File1FullName, string File2FullName)
        {

            // 두 파일 비교
            Compare(File1FullName, File2FullName);

            // 결과를 콘솔로 출력
            Show();

            // 결과를 파일로 출력
            WriteFile();
        }

        public bool Compare(string File1FullName, string File2FullName)
        {
            Console.WriteLine("Compare()");

            PFile file1 = new PFile();
            PFile file2 = new PFile();

            // 파일 Check
            if (false == file1.Check(File1FullName)) { return false; }
            if (false == file2.Check(File2FullName)) { return false; }

            //iPeek1 = 
            //file1.srFile.Peek();
            



            return true;
        }

        public void Show()
        {
            Console.WriteLine("Show()");
        }

        public void WriteFile()
        {
            Console.WriteLine("WriteFile()");
        }
    }
}
