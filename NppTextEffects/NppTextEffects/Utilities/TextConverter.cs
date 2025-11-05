using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextEffects.Utilities
{

    /// <summary>
    /// 全角・半角文字の相互変換を行う静的クラス
    /// </summary>
    public static class TextConverter
    {

        // 全角字符对照表（与原函数 zenKana1 に対応）
        private const string ZenkakuMap1 = "ヲァィゥェォャュョッーアイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワン";

        // 濁点・半濁点付き全角カタカナ对照表（と原函数 zenkana2 に対応）
        private const string ZenkakuMap2 = "ヴガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポ";

        // 半角字符对照表（と原函数 hanKanaArray1 に対応）
        private static readonly char[] HankakuMapArray1 = "ｦｧｨｩｪｫｬｭｮｯｰｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜﾝ".ToCharArray();

        // 濁点・半濁点付き半角カタカナ（と原函数 hanKanaArray2 に対応）
        private static readonly char[] HankakuMapArray2 = "ｳﾞｶﾞｷﾞｸﾞｹﾞｺﾞｻﾞｼﾞｽﾞｾﾞｿﾞﾀﾞﾁﾞﾂﾞﾃﾞﾄﾞﾊﾞﾋﾞﾌﾞﾍﾞﾎﾞﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟ".ToCharArray();


        // === 1. 半角から全角への変換 ===

        /// <summary>
        /// 半角文字（英数字、スペース、カタカナ）を全角に変換します。
        /// </summary>
        /// <param name="halfWidthString">半角文字列</param>
        /// <returns>全角文字列</returns>
        public static string ToFullWidth(string halfWidthString)
        {
            if (string.IsNullOrEmpty(halfWidthString))
            {
                return halfWidthString;
            }

            StringBuilder sb = new StringBuilder(halfWidthString.Length);

            //foreach (char c in halfWidthString)
            for (int i = 0; i < halfWidthString.Length; i++)
            {
                char currentChar = halfWidthString[i];
                // ----------------------------------------------------
                // 1. 半角英数字と一部記号
                // ----------------------------------------------------
                if (currentChar >= '!' && currentChar <= '~') // ASCII 33 ('!') から 126 ('~') の範囲
                {
                    // 全角は半角より 0xFEE0 (65248) だけオフセットがある
                    sb.Append((char)(currentChar + 0xFEE0));
                }
                // ----------------------------------------------------
                // 2. 半角スペースの変換
                // ----------------------------------------------------
                else if (currentChar == '\u0020')
                {
                    // 半角スペース (U+0020) を全角スペース (U+3000) へ
                    sb.Append('\u3000');
                }
                // ----------------------------------------------------
                // 3. 半角カタカナの変換 (JIS X 0201 の範囲)
                // ----------------------------------------------------
                else if (currentChar >= '｡' && currentChar <= 'ﾟ')
                {
                    // カタカナは複雑なマッピングが必要なため、ここでは簡単な変換ロジックを省略し、
                    // 最も汎用的な方法として、文字コードの連続性に基づく変換を行います。
                    // 実際には辞書マッピングがより正確ですが、ここでは ASCII/JIS範囲の特殊処理を優先します。

                    bool foundMatch = false;

                    // 次の文字（濁点/半濁点）が存在するかチェック
                    if (i + 1 < halfWidthString.Length)
                    {
                        char nextChar = halfWidthString[i + 1];

                        // 2文字のペアを現在のインデックスから取り出す
                        // 例: "ｳ" + "ﾞ"
                        string halfPair = currentChar.ToString() + nextChar.ToString();

                        // HankakuMapArray2 は char[] なので、検索のために文字列に変換して使用します。
                        // HankakuMapArray2 は2文字1組で全角文字と対応しています。
                        // "ｳﾞ", "ｶﾞ", "ｷﾞ", ... のように格納されていると想定します。

                        // HankakuMapArray2 は flat な char[] ですが、ここでは 2文字ずつ処理します。


                        // HankakuMapArray2 を2文字ずつインクリメントしながら検索
                        // i/2 は ZenkakuMap2 のインデックスに対応
                        for (int j = 0; j < HankakuMapArray2.Length; j += 2)
                        {
                            if (HankakuMapArray2[j] == currentChar && HankakuMapArray2[j + 1] == nextChar)
                            {
                                // 2文字がマッチした場合
                                int zenkakuIndex = j / 2;

                                // 対応する全角文字 ZenkakuMap2[j/2] を結果に追加
                                sb.Append(ZenkakuMap2[zenkakuIndex]);

                                // 2文字分進めたので、ループインデックスも1つ飛ばす
                                i++;
                                foundMatch = true;
                                break;
                            }
                        }

                    }

                    if (foundMatch)
                    {
                        continue; // マッチして処理したので、次の文字へ
                    }

                    // 例：半角カタカナの処理は複雑なため、ここでは一旦そのままにしておきます。
                    // (本格的な処理には、文字コード表に基づいた詳細なマッピングが必要です)
                    sb.Append(ConvertHalfKanaToFullKana(currentChar));
                }
                // ----------------------------------------------------
                // 4. その他（上記以外）
                // ----------------------------------------------------
                else
                {
                    // それ以外の文字はそのまま保持
                    sb.Append(currentChar);
                }
            }

            return sb.ToString();
        }

        // === 2. 全角から半角への変換 ===

        /// <summary>
        /// 全角文字（英数字、スペース、カタカナ）を半角に変換します。
        /// </summary>
        /// <param name="fullWidthString">全角文字列</param>
        /// <returns>半角文字列</returns>
        public static string ToHalfWidth(string fullWidthString)
        {
            if (string.IsNullOrEmpty(fullWidthString))
            {
                return fullWidthString;
            }

            StringBuilder sb = new StringBuilder(fullWidthString.Length);

            foreach (char c in fullWidthString)
            {
                // ----------------------------------------------------
                // 1. 全角英数字と一部記号
                // ----------------------------------------------------
                if (c >= '！' && c <= '～') // 全角ASCII範囲 (U+FF01 から U+FF5E)
                {
                    // 全角から半角は 0xFEE0 (65248) だけオフセットを引く
                    sb.Append((char)(c - 0xFEE0));
                }
                // ----------------------------------------------------
                // 2. 全角スペースの変換
                // ----------------------------------------------------
                else if (c == '\u3000') // 全角スペース (U+3000)
                {
                    // 全角スペース (U+3000) を半角スペース (U+0020) へ
                    sb.Append('\u0020');
                }
                // ----------------------------------------------------
                // 3. 全角カタカナの変換
                // ----------------------------------------------------
                else if (c >= 'ァ' && c <= 'ヶ')
                {
                    // 全角カタカナの処理は複雑なため、ここでは省略し、
                    // 詳細なマッピング関数を使用することを推奨します。
                    sb.Append(ConvertFullKanaToHalfKana(c));
                }
                // ----------------------------------------------------
                // 4. その他（上記以外）
                // ----------------------------------------------------
                else
                {
                    // それ以外の文字はそのまま保持
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }


        // ==========================================================
        // 補助関数 (カタカナの処理はマッピングが必要で複雑なため、ダミーまたは外部ライブラリを推奨)
        // ==========================================================

        /// <summary>
        /// 半角カタカナを、清音の全角カタカナ、または対応する全角記号に変換します。
        /// 濁点/半濁点の結合処理は、この関数ではなく呼び出し元で行う必要があります。
        /// </summary>
        /// <param name="halfKana">半角カタカナ文字</param>
        /// <returns>対応する全角文字。変換できない場合は元の半角文字をそのまま返します。</returns>
        private static char ConvertHalfKanaToFullKana(char halfKana)
        {
            // --- 1. Map 1 (清音、小文字、長音符など) の処理 ---
            int index1 = Array.IndexOf(HankakuMapArray1, halfKana);

            if (index1 >= 0)
            {
                // 変換表1 にマッチした場合、1文字の全角文字に変換
                return ZenkakuMap1[index1];
            }

            /*// --- 2. 濁点・半濁点単体の処理 ---
            if (halfKana == HankakuDaku)
            {
                // 濁点文字単体は、全角濁点に変換
                return ZenkakuDaku;
            }

            if (halfKana == HankakuHanDaku)
            {
                // 半濁点文字単体は、全角半濁点に変換
                return ZenkakuHanDaku;
            }*/

            // --- 3. 変換表にない文字 ---
            // カタカナ、濁点・半濁点以外の文字はそのまま返す
            return halfKana;
        }


        /// <summary>
        /// 全角カタカナを、定義されたマッピングテーブルに従い半角カタカナに変換します。
        /// </summary>
        /// <param name="fullKana">全角カタカナ文字</param>
        /// <returns>対応する半角文字（濁音/半濁音は2文字）</returns>
        private static string ConvertFullKanaToHalfKana(char fullKana)
        {
            // 文字列として処理するため、StringBuilderを使用
            StringBuilder resultBuilder = new StringBuilder();
            string fullChar = fullKana.ToString();
            int index;

            // --- 1. Map 1 (清音、小文字、長音符など) の処理 ---
            index = ZenkakuMap1.IndexOf(fullChar);
            if (index >= 0)
            {
                // 変換表1 にマッチした場合、1文字の半角文字に変換
                resultBuilder.Append(HankakuMapArray1[index]);
                return resultBuilder.ToString();
            }

            // --- 2. Map 2 (濁点・半濁点付きカタカナ) の処理 ---
            index = ZenkakuMap2.IndexOf(fullChar);
            if (index >= 0)
            {
                // 変換表2 にマッチした場合、2文字の半角表現に分解
                // 例: 'ガ' → 'ｶ' + 'ﾞ' (ただし HankakuMapArray2 の格納形式による)

                // 最初の半角文字を取得 (index * 2)
                resultBuilder.Append(HankakuMapArray2[index * 2]);

                // 2番目の半角文字（濁点/半濁点など）を取得 (index * 2 + 1)
                resultBuilder.Append(HankakuMapArray2[index * 2 + 1]);

                return resultBuilder.ToString();
            }

            // --- 3. 変換表にない文字 ---
            // 変換対象ではない文字はそのまま返す (このメソッドは文字単位処理なので、stringで返すために一旦そのままappend)
            return fullChar;
        }


    }
}
