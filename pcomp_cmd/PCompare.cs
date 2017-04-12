using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using pfile;

namespace pcompare
{
    /// <summary>
    /// 두 파일을 읽어 비교한 후 콘솔/텍스트파일로 결과를 출력하는 클래스.
    /// </summary>
    /// <remarks>
    /// created : 2017.04.10.
    /// writer  : 장민수
    /// </remarks>    
    public static class PCompare
    {
        private static int iFile1Line; // 파일1의 라인수.
        private static int iFile2Line; // 파일2의 라인수.

        /// <summary>
        /// 두 파일을 읽어 비교한 후 콘솔/텍스트파일로 결과를 출력.
        /// </summary>
        /// <param name="File1FullName">파일1 전체경로</param>
        /// <param name="File2FullName">파일2 전체경로</param>
        /// <remarks>
        /// created : 2017.04.10.
        /// writer  : 장민수
        /// </remarks>
        /// <returns>true: 정상, false: 비정상.</returns>
        public static bool Compare(string File1FullName, string File2FullName)
        {
            try
            {
                // 두 파일의 객체 생성.
                PFile file1 = new PFile(File1FullName);
                PFile file2 = new PFile(File2FullName);

                // 파일 체크.
                if (false == file1.Check()) { return false; }
                if (false == file2.Check()) { return false; }

                // StreamReader 객체 할당
                if (false == file1.SetStreamReader()) { return false; }
                if (false == file2.SetStreamReader()) { return false; }


                // 비교결과를 기록할 파일 객체 생성.
                PFile resultFile = new PFile(file1.DirectoryName + @"\Files_CompareResult.txt");

                bool bInit = false; // 초기화 여부(최초 1회만 수행. 파일 생성 및 초기화).
                string strFile1Line = ""; // 파일1의 라인내용.
                string strFile2Line = ""; // 파일2의 라인내용.
                iFile1Line = 0; // 파일1의 라인수.
                iFile2Line = 0; // 파일2의 라인수.

                // 파일1, 파일2의 끝에 도달할 때까지 반복하여 수행.
                while ((!file1.StreamReader.EndOfStream) || (!file2.StreamReader.EndOfStream))
                {
                    // Read 파일1 Line
                    strFile1Line = ReadLine(file1, 1);
                    if (null == strFile1Line) { return false; }

                    // Read 파일2 Line
                    strFile2Line = ReadLine(file2, 2);
                    if (null == strFile2Line) { return false; }

                    ////////////////////////////////////////////////////////////////////////////////
                    // 두 라인을 비교하여 다를경우, 콘솔 및 텍스트파일에 출력(공백라인 무시).
                    ////////////////////////////////////////////////////////////////////////////////
                    if ("" != strFile1Line && "" != strFile2Line
                        && strFile1Line != strFile2Line)
                    {
                        // 텍스트파일을 초기화하지 않았을 경우(최초 1회 수행시) 텍스트파일 초기화, 체크, 객체 할당.
                        if (false == bInit)
                        {
                            // 텍스트파일 초기화.
                            if (false == resultFile.InitTextFile()) { return false; }

                            // 텍스트파일 체크.
                            if (false == resultFile.Check()) { return false; }

                            // StreamWriter 객체 할당
                            if (false == resultFile.SetStreamWriter()) { return false; }

                            // 초기화 후엔 true로 수정.
                            bInit = true;
                        }

                        // 비교결과를 포맷팅하여 StringBuilder에 저장.
                        StringBuilder data = ResultFormat(iFile1Line, strFile1Line, iFile2Line, strFile2Line);

                        // 비교결과를 콘솔에 출력.
                        Console.Write(data);

                        // 비교결과를 파일에 출력.
                        resultFile.WriteTextFile(data);
                    }
                    ////////////////////////////////////////////////////////////////////////////////
                }

                // StreamReader 객체 내부스트림 Close
                file1.CloseStreamReader();
                file2.CloseStreamReader();

                // 두 파일의 내용이 다른게 있을 경우(객체 할당을 했을 경우) 내부스트림 Close
                if (true == bInit)
                {
                    resultFile.CloseStreamWriter();
                }
                // 두 파일의 내용이 같을 경우
                else
                {
                    Console.WriteLine("Files are equals.");
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
        /// 파일의 한 줄을 읽어서 String 타입으로 반환함(공백라인 무시).
        /// </summary>
        /// <param name="file">파일정보가 담긴 PFile 객체</param>
        /// <param name="selectFile">파일1: 1, 파일2: 2</param>
        /// <returns>파일에서 읽어들인 라인의 데이터 내용(String)</returns>
        private static string ReadLine(PFile file, int selectFile)
        {
            string strFileLine = "";
            try
            {
                if (selectFile != 1 && selectFile != 2)
                {
                    throw (new Exception("ReadLine 함수 호출 시, selectFile 인자값은 1,2 중에 선택하세요."));
                }

                do
                {
                    // 다음 문자로 Peek
                    file.StreamReader.Peek();

                    // 파일의 끝이 아니면, 해당 라인을 읽어 string변수에 저장.
                    if (!file.StreamReader.EndOfStream)
                    {
                        // 파일1 라인수 카운트.
                        if (selectFile == 1) { iFile1Line++; }
                        // 파일2 라인수 카운트.
                        else if (selectFile == 2) { iFile2Line++; }

                        // 한 라인을 읽어 string변수에 저장.
                        strFileLine = file.StreamReader.ReadLine();

                        // 공백라인 무시.
                        if ("" != strFileLine) { break; }

                        // 마지막라인이 공백라인이면 문구 셋팅 후 break;
                        if (("" == strFileLine) && file.StreamReader.EndOfStream)
                        {
                            // 파일1 라인수 0으로 초기화.
                            if (selectFile == 1) { iFile1Line = 0; }
                            // 파일2 라인수 0으로 초기화.
                            else if (selectFile == 2) { iFile2Line = 0; }

                            // '#####<EMPTY>#####' 문구로 셋팅
                            strFileLine = "#####<EMPTY>#####";
                            break;
                        }
                    }
                    else
                    {
                        // 파일1 라인수 0으로 초기화.
                        if (selectFile == 1) { iFile1Line = 0; }
                        // 파일2 라인수 0으로 초기화.
                        else if (selectFile == 2) { iFile2Line = 0; }

                        // '#####<EMPTY>#####' 문구로 셋팅
                        strFileLine = "#####<EMPTY>#####";
                    }
                } while (!file.StreamReader.EndOfStream); // 파일1의 끝까지 반복하여 수행.
            }
            catch (Exception e)
            {
                Console.WriteLine("예외 발생:{0}", e.Message);
                return null;
            }
            return strFileLine;
        }


        /// <summary>
        /// 출력할 내용의 양식을 구성함.
        /// </summary>
        /// <param name="iFile1Line">파일1 라인번호</param>
        /// <param name="strFile1Line">파일1 라인내용</param>
        /// <param name="iFile2Line">파일2 라인번호</param>
        /// <param name="strFile2Line">파일2 라인내용</param>
        /// <returns>true: 정상, false: 비정상.</returns>
        private static StringBuilder ResultFormat(int iFile1Line, string strFile1Line, int iFile2Line, string strFile2Line)
        {
            StringBuilder sbData = new StringBuilder();

            /*
                /// 2017.04.12.현재 양식 ///
                -----------------------------------------------------------------------------------
                [File1 (라인번호)]	파일1 내용
                [File2 (라인번호)]	파일2 내용
            */

            sbData.Append("-----------------------------------------------------------------------------------\r\n");
            sbData.Append("[File1 (");
            sbData.Append(iFile1Line);
            sbData.Append(")]\t");
            sbData.Append(strFile1Line);
            sbData.Append("\r\n");
            sbData.Append("[File2 (");
            sbData.Append(iFile2Line);
            sbData.Append(")]\t");
            sbData.Append(strFile2Line);
            sbData.Append("\r\n");

            return sbData;
        }

    }
}
