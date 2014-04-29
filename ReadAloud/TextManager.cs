// Copyright (c) 2014 Amandeep Singh

//  The MIT License (MIT)
//  Permission is hereby granted, free of charge, to any person obtaining a
//  copy of this software and associated documentation files (the "Software"),
//  to deal in the Software without restriction, including without limitation
//  the rights to use, copy, modify, merge, publish, distribute, sublicense,
//  and/or sell copies of the Software, and to permit persons to whom the
//  Software is furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included
//  in all copies or substantial portions of the Software.

//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//  OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//  DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Read_Aloud
{
    class TextManager
    {
        #region INTERNALS
        #region SINGLETON
        private static TextManager instance = new TextManager();
        public static TextManager Instance { get { return instance; } }
        #endregion

        private TextManager()
        {
            regexes.Add(new RegexReplacement(new Regex("(\\w)"+
                "[\\u2010\\u2011\\u00AD\\u002D]" +
                "\\s+(\\w)"), "$1$2"));
            regexes.Add(new RegexReplacement(new Regex("(\\w)\r\n(\\w)"), "$1 $2"));
        }

        class RegexReplacement
        {
            public readonly Regex regex;
            public readonly string replacement;
            public RegexReplacement(Regex regex, string replacement)
            {
                this.regex = regex;
                this.replacement = replacement;
            }
        }
        List<RegexReplacement> regexes = new List<RegexReplacement>();
#endregion

        public string Process(string text)
        {
            foreach (var regex in regexes)
                text = regex.regex.Replace(text, regex.replacement);
            return text;
        }
    }
}
