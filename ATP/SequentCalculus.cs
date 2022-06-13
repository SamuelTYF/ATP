using System;
using System.Collections.Generic;

namespace ATP
{
    public class SequentCalculus
    {
        public List<ITerm> Left;
        public List<ITerm> Right;
        public HashSet<string> LeftVariables;
        public HashSet<string> RightVariables;
        public List<SequentCalculus> Nexts;
        public Mode Mode;
        public SequentCalculus()
        {
            Left = new List<ITerm>();
            Right = new List<ITerm>();
            LeftVariables = new HashSet<string>();
            RightVariables = new HashSet<string>();
            Nexts = new List<SequentCalculus>();
            Mode = Mode.None;
        }
        public void RegisterLeft(ITerm term)
        {
            string str = term.ToString();
            if(!LeftVariables.Contains(str))
            {
                Left.Add(term);
                LeftVariables.Add(str);
                if (RightVariables.Contains(str)) Mode = Mode.Success;
            }
        }
        public void RegisterRight(ITerm term)
        {
            string str = term.ToString();
            if (!RightVariables.Contains(str))
            {
                Right.Add(term);
                RightVariables.Add(str);
                if (LeftVariables.Contains(str)) Mode = Mode.Success;
            }
        }
        public void Reduce()
        {
            if (Mode == Mode.Success) return;
            for (int i = 0; i < Left.Count; i++)
                if (Left[i] is And)
                {
                    LAnd(i);
                    return;
                }
                else if (Left[i] is Not)
                {
                    LNot(i);
                    return;
                }
            for (int i = 0; i < Right.Count; i++)
                if (Right[i] is Or)
                {
                    ROr(i);
                    return;
                }
                else if (Right[i] is Implies)
                {
                    RImplies(i);
                    return;
                }
                else if (Right[i] is Not)
                {
                    RNot(i);
                    return;
                }
            int left = 0;
            for (int i = 1; i < Left.Count; i++)
                if (Left[i].Deep > Left[left].Deep)
                    left = i;
            int right = 0;
            for (int i = 1; i < Right.Count; i++)
                if (Right[i].Deep > Right[right].Deep)
                    right = i;
            if (Right.Count == 0 || Left.Count > 0 && Left[left].Deep >= Right[right].Deep)
            {
                if (Left[left] is Or)
                {
                    LOr(left);
                    return;
                }
                else if (Left[left] is Implies)
                {
                    LImplies(left);
                    return;
                }
                else throw new Exception($"Reduce Error: unexcept type of {Left[left]}");
            }
            else
            {
                if (Right[right] is And)
                {
                    RAnd(right);
                    return;
                }
                else throw new Exception($"Reduce Error: unexcept type of {Right[right]}");
            }
        }
        public void LAnd(int index)
        {
            ITerm term = Left[index];
            if (term is And and)
            {
                SequentCalculus result = new SequentCalculus();
                for (int i = 0; i < index; i++) result.RegisterLeft(Left[i]);
                for (int i = index + 1; i < Left.Count; i++) result.RegisterLeft(Left[i]);
                for (int i = 0; i < Right.Count; i++) result.RegisterRight(Right[i]);
                for (int i = 0; i < and.Terms.Length; i++)
                    result.RegisterLeft(and.Terms[i]);
                Nexts.Add(result);
                result.Reduce();
                Mode = result.Mode;
            }
            else throw new System.Exception($"LAnd Error for {term} is not an And Term");
        }
        public void ROr(int index)
        {
            ITerm term = Right[index];
            if (term is Or or)
            {
                SequentCalculus result = new SequentCalculus();
                for (int i = 0; i < Left.Count; i++) result.RegisterLeft(Left[i]);
                for (int i = 0; i < index; i++) result.RegisterRight(Right[i]);
                for (int i = index + 1; i < Right.Count; i++) result.RegisterRight(Right[i]);
                for (int i = 0; i < or.Terms.Length; i++)
                    result.RegisterRight(or.Terms[i]);
                Nexts.Add(result);
                result.Reduce();
                Mode = result.Mode;
            }
            else throw new System.Exception($"ROr Error for {term} is not an Or Term");
        }
        public void LOr(int index)
        {
            ITerm term = Left[index];
            if(term is Or or)
            {
                for(int j=0;j<or.Terms.Length; j++)
                {
                    SequentCalculus result = new SequentCalculus();
                    for (int i = 0; i < index; i++) result.RegisterLeft(Left[i]);
                    for (int i = index + 1; i < Left.Count; i++) result.RegisterLeft(Left[i]);
                    for (int i = 0; i < Right.Count; i++) result.RegisterRight(Right[i]);
                    result.RegisterLeft(or.Terms[j]);
                    Nexts.Add(result);
                }
                for(int j=0;j<Nexts.Count;j++)
                {
                    Nexts[j].Reduce();
                    if (Nexts[j].Mode==Mode.Fail)
                    {
                        Mode = Mode.Fail;
                        return;
                    }
                }
                Mode = Mode.Success;
            }
            else throw new System.Exception($"LOr Error for {term} is not an Or Term");
        }
        public void RAnd(int index)
        {
            ITerm term = Right[index];
            if (term is And and)
            {
                for (int j = 0; j < and.Terms.Length; j++)
                {
                    SequentCalculus result = new SequentCalculus();
                    for (int i = 0; i < Left.Count; i++) result.RegisterLeft(Left[i]);
                    for (int i = 0; i < index; i++) result.RegisterRight(Right[i]);
                    for (int i = index + 1; i < Right.Count; i++) result.RegisterRight(Right[i]);
                    result.RegisterRight(and.Terms[j]);
                    Nexts.Add(result);
                }
                for (int j = 0; j < Nexts.Count; j++)
                {
                    Nexts[j].Reduce();
                    if (Nexts[j].Mode == Mode.Fail)
                    {
                        Mode = Mode.Fail;
                        return;
                    }
                }
                Mode = Mode.Success;
            }
            else throw new System.Exception($"RAnd Error for {term} is not an And Term");
        }
        public void LImplies(int index)
        {
            ITerm term = Left[index];
            if (term is Implies implies)
            {
                for (int j = 0; j < 2; j++)
                {
                    SequentCalculus result = new SequentCalculus();
                    for (int i = 0; i < index; i++) result.RegisterLeft(Left[i]);
                    for (int i = index + 1; i < Left.Count; i++) result.RegisterLeft(Left[i]);
                    for (int i = 0; i < Right.Count; i++) result.RegisterRight(Right[i]);
                    Nexts.Add(result);
                }
                Nexts[0].RegisterRight(implies.Left);
                Nexts[1].RegisterLeft(implies.Right);
                for (int j = 0; j < Nexts.Count; j++)
                {
                    Nexts[j].Reduce();
                    if (Nexts[j].Mode == Mode.Fail)
                    {
                        Mode = Mode.Fail;
                        return;
                    }
                }
                Mode = Mode.Success;
            }
            else throw new System.Exception($"LImplies Error for {term} is not a Implies Term");
        }
        public void RImplies(int index)
        {
            ITerm term = Right[index];
            if (term is Implies implies)
            {
                SequentCalculus result = new SequentCalculus();
                for (int i = 0; i < Left.Count; i++) result.RegisterLeft(Left[i]);
                for (int i = 0; i < index; i++) result.RegisterRight(Right[i]);
                for (int i = index + 1; i < Right.Count; i++) result.RegisterRight(Right[i]);
                result.RegisterLeft(implies.Left);
                result.RegisterRight(implies.Right);
                Nexts.Add(result);
                result.Reduce();
                Mode = result.Mode;
            }
            else throw new System.Exception($"RImplies Error for {term} is not a Implies Term");
        }
        public void LNot(int index)
        {
            ITerm term = Left[index];
            if (term is Not not)
            {
                SequentCalculus result = new SequentCalculus();
                for (int i = 0; i < index; i++) result.RegisterLeft(Left[i]);
                for (int i = index + 1; i < Left.Count; i++) result.RegisterLeft(Left[i]);
                for (int i = 0; i < Right.Count; i++) result.RegisterRight(Right[i]);
                result.RegisterRight(not.Term);
                Nexts.Add(result);
                result.Reduce();
                Mode = result.Mode;
            }
            else throw new System.Exception($"LNot Error for {term} is not a Not Term");
        }
        public void RNot(int index)
        {
            ITerm term = Right[index];
            if (term is Not not)
            {
                SequentCalculus result = new SequentCalculus();
                for (int i = 0; i < Left.Count; i++) result.RegisterLeft(Left[i]);
                for (int i = 0; i < index; i++) result.RegisterRight(Right[i]);
                for (int i = index + 1; i < Right.Count; i++) result.RegisterRight(Right[i]);
                result.RegisterLeft(not.Term);
                Nexts.Add(result);
                result.Reduce();
                Mode = result.Mode;
            }
            else throw new System.Exception($"RNot Error for {term} is not a Not Term");
        }
        public string FormatPrint()
        {
            string[] left = new string[Left.Count];
            for(int i=0;i<left.Length;i++)
                left[i]=Left[i].ToString();
            string[] right = new string[Right.Count];
            for (int i = 0; i < right.Length; i++)
                right[i] = Right[i].ToString();
            if (Mode==Mode.None) return $"{string.Join(",",left)} \\vdash {string.Join(",", right)}";
            else
            {
                string[] lines = new string[Nexts.Count];
                for (int i = 0; i < lines.Length; i++)
                    lines[i] = Nexts[i].FormatPrint();
                return $"\\cfrac{{{string.Join(",", left)} \\vdash {string.Join(",", right)}}}{{{string.Join(" \\qquad ", lines)}}}";
            }
        }
    }
}
