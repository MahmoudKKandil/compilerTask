using System.Collections.Generic;
using System.Text.RegularExpressions;

public enum Token_Class
{
    Begin, Call, Declare, End, Do, Else, EndIf, EndUntil, EndWhile, If, Integer,
    Parameters, Procedure, Program, Read, Real, Set, Then, Until, While, Write,
    Dot, Semicolon, Comma, LParanthesis, RParanthesis, EqualOp, LessThanOp,
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp,
    Idenifier, Constant, RightBraces, LeftBraces, DataTypeINT, Comment,
    Repeat, Return, Assign, String
}
namespace JASON_Compiler
{
    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {

            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("begin", Token_Class.Begin);
            ReservedWords.Add("call", Token_Class.Call);
            ReservedWords.Add("declare", Token_Class.Declare);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("do", Token_Class.Do);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("endif", Token_Class.EndIf);
            ReservedWords.Add("enduntil", Token_Class.EndUntil);
            ReservedWords.Add("endwhile", Token_Class.EndWhile);
            ReservedWords.Add("integer", Token_Class.Integer);
            ReservedWords.Add("parameters", Token_Class.Parameters);
            ReservedWords.Add("procedure", Token_Class.Procedure);
            ReservedWords.Add("program", Token_Class.Program);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("real", Token_Class.Real);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("set", Token_Class.Set);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("while", Token_Class.While);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("int", Token_Class.DataTypeINT);

            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("!=", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("{", Token_Class.LeftBraces);
            Operators.Add("}", Token_Class.RightBraces);
            Operators.Add(":=", Token_Class.Assign);

        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\t' || CurrentChar == '\n')
                    continue;

                if (CurrentChar >= 'A' && CurrentChar <= 'z') //if you read a character
                {
                    j++;
                    if (j < SourceCode.Length)
                        while (SourceCode[j] >= 'A' && SourceCode[j] <= 'z' || SourceCode[j] >= '0' && SourceCode[j] <= '9')
                        {

                            CurrentLexeme += SourceCode[j];
                            j++;
                            if (j >= SourceCode.Length)
                                break;
                        }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;

                }

                else if (CurrentChar >= '0' && CurrentChar <= '9')
                {
                    j++;
                    if (j < SourceCode.Length)
                        while (SourceCode[j] >= '0' && SourceCode[j] <= '9' || SourceCode[j] == '.')
                        {

                            CurrentLexeme += SourceCode[j];
                            j++;
                            if (j >= SourceCode.Length)
                                break;
                        }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }
                else if (CurrentChar == '/')
                {
                    j++;
                    if (j < SourceCode.Length)
                        if (SourceCode[j] == '*')
                        {
                            CurrentLexeme += SourceCode[j];
                            j++;
                            while (!CurrentLexeme.Contains("*/"))
                            {
                                CurrentLexeme += SourceCode[j];
                                j++;
                                if (j >= SourceCode.Length)
                                    break;
                            }
                        }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }
                else if (CurrentChar == '/')
                {
                    j++;
                    if (j < SourceCode.Length)
                        if (SourceCode[j] == '*')
                        {
                            CurrentLexeme += SourceCode[j];
                            j++;
                            while (!CurrentLexeme.Contains("*/"))
                            {
                                CurrentLexeme += SourceCode[j];
                                j++;
                                if (j >= SourceCode.Length)
                                    break;
                            }
                        }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }
                else if (CurrentChar == '"')
                {
                    j++;
                    if (j < SourceCode.Length)
                        while (SourceCode[j] != '"')
                        {
                            CurrentLexeme += SourceCode[j];
                            j++;
                        }
                    CurrentLexeme += SourceCode[j];
                    FindTokenClass(CurrentLexeme);
                    i = j;
                }
                else if (CurrentChar == ':')
                {
                    j++;
                    if (j < SourceCode.Length)
                        if (SourceCode[j] == '=')
                        {
                            CurrentLexeme += SourceCode[j];
                        }
                    FindTokenClass(CurrentLexeme);
                    i = j ;
                }
                else if (CurrentChar == '!')
                {
                    j++;
                    if (j < SourceCode.Length)
                        if (SourceCode[j] == '=')
                        {
                            CurrentLexeme += SourceCode[j];
                        }
                    FindTokenClass(CurrentLexeme);
                    i = j;
                }
                else
                {
                    FindTokenClass(CurrentChar.ToString());
                }
            }

            JASON_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            //Is it an operator?
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }
            //Is it a Comment?
            else if (isComment(Lex))
            {
                Tok.token_type = Token_Class.Comment;
                Tokens.Add(Tok);
            }
            //Is it an identifier?
            else if (isString(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tokens.Add(Tok);
            }
            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
            }

            //Is it a Constant?
            else if (isConstant(Lex))
            {
                Tok.token_type = Token_Class.Constant;
                Tokens.Add(Tok);
            }
            
            //Is it an undefined?
            else
            {
                Errors.Error_List.Add(Lex + " Is undefined");
            }
        }

        bool isComment(string lex)
        {
            var regex = new Regex(@"^\/\*[\w\s@./#<=>:&+\-[\]]*\]*\*\/$");
            return regex.IsMatch(lex);
        }
        bool isString(string lex)
        {
            var regex = new Regex("^\"[\\w\\s@./#<=>:&+\\-[\\]]*\"$");
            return regex.IsMatch(lex);
        }
        bool isIdentifier(string lex)
        {
            var regex = new Regex("^[A-z]([A-z]|\\d)*$");
            return regex.IsMatch(lex);
        }
        bool isConstant(string lex)
        {
            var regex = new Regex("^(\\d+[.])?(\\d)+$");
            return regex.IsMatch(lex);
        }
    }
}
