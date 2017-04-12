using System;
using System.Text;
using System.IO;


namespace pfile
{
    /// <summary>
    /// 파일의 정보를 갖고, Init, Read, Write, Check 기능을 하는 클래스.
    /// </summary>
    /// <remarks>
    /// created : 2017.04.10.
    /// writer  : 장민수
    /// </remarks>
    class PFile
    {
        // 파일의 정보
        private FileInfo fi;
        private StreamReader srFile;
        private StreamWriter swFile;


        /// <summary>
        /// 파일정보 객체를 생성하는 생성자.
        /// </summary>
        /// <remarks>
        /// created : 2017.04.10.
        /// writer  : 장민수
        /// </remarks>
        /// <param name="FullName">전체경로</param>
        public PFile(string FullName)
        {
            // 객체 할당.
            fi = new FileInfo(FullName);
//Console.WriteLine("fi.FullName: ({0})\nfi.Directory: ({1})\nfi.Name: ({2})\nfi.Extension: ({3})\nfi.DirectoryName: ({4})\nfi.Length: ({5})\nfi.ToString(): ({6})\n", fi.FullName, fi.Directory, fi.Name, fi.Extension, fi.DirectoryName, fi.Length, fi.ToString());
        }


        /// <summary>
        /// 파일의 정보를 담는 FileInfo Get.
        /// </summary>
        /// <remarks>
        /// created : 2017.04.10.
        /// writer  : 장민수
        /// </remarks>
        public FileInfo FileInfo
        {
            get { return this.fi; }
        }


        /// <summary>
        /// 파일의 내용을 담는 StreamReader Get.
        /// </summary>
        /// <remarks>
        /// created : 2017.04.10.
        /// writer  : 장민수
        /// </remarks>
        public StreamReader StreamReader
        {
            get { return this.srFile; }
        }


        /// <summary>
        /// 파일의 내용을 담는 StreamReader Set.
        /// </summary>
        /// <remarks>
        /// created : 2017.04.10.
        /// writer  : 장민수
        /// </remarks>
        /// <returns>true: 정상, false: 비정상.</returns>
        public bool SetStreamReader()
        {
            try
            {
                // 파일을 읽어 Buffer에 저장함(한글 인코딩 문제로 "euc-kr"을 Default 지정).
                srFile = new StreamReader(fi.FullName, Encoding.GetEncoding("euc-kr"));
            }
            catch (Exception e)
            {
                Console.WriteLine("예외 발생:{0}", e.Message);
                return false;
            }
            return true;
        }


        /// <summary>
        /// 파일에 내용을 쓰는 StreamWriter Get.
        /// </summary>
        /// <remarks>
        /// created : 2017.04.10.
        /// writer  : 장민수
        /// </remarks>
        public StreamWriter StreamWriter
        {
            get { return this.swFile; }
        }


        /// <summary>
        /// 파일에 내용을 쓰는 StreamWriter Set.
        /// </summary>
        /// <remarks>
        /// created : 2017.04.10.
        /// writer  : 장민수
        /// </remarks>
        /// <returns>true: 정상, false: 비정상.</returns>
        public bool SetStreamWriter()
        {
            try
            {
                // 기존 파일의 내용에 텍스트를 추가.
                swFile = fi.AppendText();
            }
            catch (Exception e)
            {
                Console.WriteLine("예외 발생:{0}", e.Message);
                return false;
            }
            return true;
        }
        

        /// <summary>
        /// StreamReader 개체와 내부 스트림을 닫고 판독기와 관련된 모든 시스템 리소스를 해제합니다.
        /// </summary>
        public void CloseStreamReader()
        {
            this.srFile.Close();
        }


        /// <summary>
        /// StreamWriter 개체 및 내부스트림을 닫습니다.
        /// </summary>
        public void CloseStreamWriter()
        {
            this.swFile.Close();
        }


        /// <summary>
        /// 파일의 경로(디렉토리) Get.
        /// </summary>
        /// <remarks>
        /// created : 2017.04.10.
        /// writer  : 장민수
        /// </remarks>
        /// <returns>파일의 경로(디렉토리)</returns>
        public string DirectoryName
        {
            get { return this.fi.DirectoryName; }
        }


        /// <summary>
        /// 해당 파일의 존재여부 및 파일이 비어있는지 여부 체크.
        /// </summary>
        /// <remarks>
        /// created : 2017.04.10.
        /// writer  : 장민수
        /// </remarks>
        /// <returns>true: 정상, false: 비정상.</returns>
        public bool Check()
        {
            try
            {
                // 디렉토리 존재여부 체크.
                if (!fi.Directory.Exists)
                {
                    Console.WriteLine("해당 디렉토리가 존재하지 않습니다.({0})", fi.FullName);
                    return false;
                }
                // 파일 존재여부 체크.
                if (!fi.Exists)
                {
                    Console.WriteLine("해당 파일이 존재하지 않습니다.({0})", fi.FullName);
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


        /// <summary>
        /// 파일 생성 및 초기화
        /// </summary>
        /// <remarks>
        /// created : 2017.04.10.
        /// writer  : 장민수
        /// </remarks>
        /// <returns>true: 정상, false: 비정상.</returns>
        public bool InitTextFile()
        {
            try
            {
                FileStream fsFile = fi.OpenWrite();
                fsFile.SetLength(0);
                fsFile.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("예외 발생:{0}", e.Message);
                return false;
            }
            return true;
        }


        /// <summary>
        /// 파일에 text를 Write 함.
        /// </summary>
        /// <param name="data">파일에 기록할 내용</param>
        /// <returns>true: 정상, false: 비정상.</returns>
        public bool WriteTextFile(StringBuilder data)
        {
            try
            {
                swFile.WriteLine(data);
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
