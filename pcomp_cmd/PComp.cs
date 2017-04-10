using System;
using System.Text;
using System.IO;


namespace pcomp
{

    /// <summary>
    /// 파일의 정보를 가지고, 유효여부를 체크하는 클래스.
    /// </summary>
    /// <remarks>
    /// created : 2017.04.10.
    /// writer  : 장민수
    /// </remarks>
    class PFile
    {
        // 파일의 정보
        public FileInfo fi;
        // 파일의 내용
        public StreamReader srFile;


        /// <summary>
        /// 해당 파일의 존재여부 및 파일이 비어있는지 여부 체크.
        /// </summary>
        /// <param name="FullName"></param>
        /// <remarks>
        /// created : 2017.04.10.
        /// writer  : 장민수
        /// </remarks>
        /// <returns>true: 정상, false: 비정상.</returns>
        public bool Check(string FullName)
        {
            try
            {
                // 파일의 객체 생성.
                fi = new FileInfo(FullName);
//Console.WriteLine("fi.FullName: ({0})\nfi.Directory: ({1})\nfi.Name: ({2})\nfi.Extension: ({3})\nfi.DirectoryName: ({4})\nfi.Length: ({5})\nfi.ToString(): ({6})\n", fi.FullName, fi.Directory, fi.Name, fi.Extension, fi.DirectoryName, fi.Length, fi.ToString());

                // 디렉토리 존재여부 체크.
                if (!fi.Directory.Exists)
                {
                    Console.WriteLine("해당 디렉토리가 존재하지 않습니다.({0})", FullName);
                    return false;
                }
                // 파일 존재여부 체크.
                if (!fi.Exists)
                {
                    Console.WriteLine("해당 파일이 존재하지 않습니다.({0})", FullName);
                    return false;
                }

                // 파일을 읽어 Buffer에 저장함(한글 인코딩 문제로 "euc-kr"을 Default 지정).
                srFile = new StreamReader(fi.FullName, Encoding.GetEncoding("euc-kr"));

                // 파일의 내용이 비어있는지 체크.
                if (null == srFile)
                {
                    Console.WriteLine("[{0}]은(는) 비어있는 파일입니다.", fi.FullName);
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


    /// <summary>
    /// 두 파일을 읽어 비교한 후 콘솔/텍스트파일로 결과를 출력하는 클래스.
    /// </summary>
    /// <remarks>
    /// created : 2017.04.10.
    /// writer  : 장민수
    /// </remarks>
    class PCompare
    {
        // 비교결과를 작성할 파일의 정보
        private FileInfo fi;


        /// <summary>
        /// 두 파일을 비교
        /// </summary>
        /// <param name="File1FullName"></param>
        /// <param name="File2FullName"></param>
        /// <remarks>
        /// created : 2017.04.10.
        /// writer  : 장민수
        /// </remarks>
        /// <returns>true: 정상, false: 비정상.</returns>
        public bool Compare(string File1FullName, string File2FullName)
        {
            try
            {
                // 두 파일의 객체 생성.
                PFile file1 = new PFile();
                PFile file2 = new PFile();

                // 파일 체크.
                if (false == file1.Check(File1FullName)) { return false; }
                if (false == file2.Check(File2FullName)) { return false; }


                int iFile1Line = 0; // 파일1의 라인수.
                int iFile2Line = 0; // 파일2의 라인수.
                string strFile1Line = ""; // 파일1의 해당 라인의 내용.
                string strFile2Line = ""; // 파일2의 해당 라인의 내용.
                bool bFirst = true; // 최초 1회만 동작 파일생성 or 파일초기화.

                // 파일1, 파일2의 끝까지 반복하여 수행.
                while ((!file1.srFile.EndOfStream) || (!file2.srFile.EndOfStream)) // EOF까지
                {
                    ////////////////////////////////////////////////////////////////////////////////
                    // Read File1 Line
                    ////////////////////////////////////////////////////////////////////////////////
                    do
                    {
                        // 다음 문자로 Peek
                        file1.srFile.Peek();

                        // 파일의 끝이 아니면, 해당 라인을 읽어 string변수에 저장.
                        if (!file1.srFile.EndOfStream)
                        {
                            // 파일 라인수 카운트.
                            iFile1Line++;

                            // 한 라인을 읽어 string변수에 저장.
                            strFile1Line = file1.srFile.ReadLine();
                            
                            // 공백라인 무시.
                            if ("" != strFile1Line) { break; }
                        }
                        else
                        {
                            //file1.srFile.Peek();

                            // 파일1을 끝까지 다 읽었을 경우
                            // 라인 수 0으로 초기화, '#####<EMPTY>#####' 문구로 셋팅
                            iFile1Line = 0;
                            strFile1Line = "#####<File1 EMPTY>#####";
                        }
                    } while (!file1.srFile.EndOfStream); // 파일1의 끝까지 반복하여 수행.
                    ////////////////////////////////////////////////////////////////////////////////


                    ////////////////////////////////////////////////////////////////////////////////
                    // Read File2 Line
                    ////////////////////////////////////////////////////////////////////////////////
                    do
                    {
                        // 다음 문자로 Peek
                        file2.srFile.Peek();

                        // 파일의 끝이 아니면, 해당 라인을 읽어 string변수에 저장.
                        if (!file2.srFile.EndOfStream)
                        {
                            // 파일 라인수 카운트.
                            iFile2Line++;

                            // 한 라인을 읽어 string변수에 저장.
                            strFile2Line = file2.srFile.ReadLine();

                            // 공백라인 무시.
                            if ("" != strFile2Line) { break; }
                        }
                        else
                        {
                            //file2.srFile.Peek();

                            // 파일2를 끝까지 다 읽었을 경우
                            // 라인 수 0으로 초기화, '#####<EMPTY>#####' 문구로 셋팅
                            iFile2Line = 0;
                            strFile2Line = "#####<File2 EMPTY>#####";
                        }
                    } while (!file2.srFile.EndOfStream); // 파일2의 끝까지 반복하여 수행.
                    ////////////////////////////////////////////////////////////////////////////////

                    ////////////////////////////////////////////////////////////////////////////////
                    // 두 라인을 비교하여 다를경우, 콘솔 및 텍스트파일에 출력(공백라인 무시).
                    ////////////////////////////////////////////////////////////////////////////////
                    if ("" != strFile1Line && "" != strFile2Line && strFile1Line != strFile2Line)
                    {
                        // 최초 1회 수행시 파일생성 또는 파일초기화
                        if (true == bFirst)
                        {
                            // 파일 초기화.
                            InitFile(file1.fi.DirectoryName);
                            bFirst = false;
                        }
                    
                        // 비교결과를 콘솔에 출력.
                        ShowFile(iFile1Line, strFile1Line, iFile2Line, strFile2Line);
                        // 비교결과를 파일에 출력.
                        WriteFile(iFile1Line, strFile1Line, iFile2Line, strFile2Line);
                    }
                    ////////////////////////////////////////////////////////////////////////////////
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("예외 발생:{0}", e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 파일 생성 및 초기화
        /// </summary>
        /// <param name="path"></param>
        /// <remarks>
        /// created : 2017.04.10.
        /// writer  : 장민수
        /// </remarks>
        /// <returns>true: 정상, false: 비정상.</returns>
        public bool InitFile(string path)
        {
            try
            {
                fi = new FileInfo(path + @"\Files_CompareResult.txt");

                // 파일이 존재하지 않으면 텍스트파일 생성
                if (!fi.Exists)
                {
                    fi.CreateText();
                }
                // 텍스트파일이 존재하면 파일의 내용을 초기화
                else
                {
                    FileStream fsFile = fi.OpenWrite();
                    fsFile.SetLength(0);
                    fsFile.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("예외 발생:{0}", e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 콘솔에 라인과 내용을 포맷팅하여 출력.
        /// </summary>
        /// <param name="iFile1Line"></param>
        /// <param name="strFile1Line"></param>
        /// <param name="iFile2Line"></param>
        /// <param name="strFile2Line"></param>
        /// <remarks>
        /// created : 2017.04.10.
        /// writer  : 장민수
        /// </remarks>
        public void ShowFile(int iFile1Line, string strFile1Line, int iFile2Line, string strFile2Line)
        {
            Console.Write("-----------------------------------------------------------------------------------\r\n");
            Console.Write("[File1 ({0})]\t{1}\r\n", iFile1Line, strFile1Line);
            Console.Write("[File2 ({0})]\t{1}\r\n", iFile2Line, strFile2Line);
        }

        /// <summary>
        /// 출력파일에 라인과 내용을 포맷팅하여 출력.
        /// </summary>
        /// <param name="file1LineNum"></param>
        /// <param name="file1Line"></param>
        /// <param name="file2LineNum"></param>
        /// <param name="file2Line"></param>
        /// <returns></returns>
        public bool WriteFile(int iFile1Line, string strFile1Line, int iFile2Line, string strFile2Line)
        {
            try
            {
                StreamWriter swFile = fi.AppendText();
                swFile.WriteLine("-----------------------------------------------------------------------------------");
                swFile.WriteLine("[File1 ({0})]\t{1}", iFile1Line, strFile1Line);
                swFile.WriteLine("[File2 ({0})]\t{1}", iFile2Line, strFile2Line);
                swFile.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("예외 발생:{0}", e.Message);
                return false;
            }
            return true;
        }
    }
}
