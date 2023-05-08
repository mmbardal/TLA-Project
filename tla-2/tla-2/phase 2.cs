using System;
using System.Collections.Generic;
using System.Linq;


using Newtonsoft.Json;
//Console.WriteLine("isdehfopihepi");
namespace phase2
{
    class Graph
    {
        public int V; // تعداد گره‌ها
        public List<int>[] adj; // لیست مجاورت

        public Graph(int v)
        {
            V = v;
            adj = new List<int>[V];

            for (int i = 0; i < V; i++)
            {
                adj[i] = new List<int>();
            }
        }

        // اضافه کردن یک یال با دو گره v و w
        public void addEdge(int v, int w)
        {
            adj[v].Add(w);
            adj[w].Add(v);
        }

        // به دنبال خروجی طول کوتاه برای رسیدن به v از u با استفاده از Depth-First-Search (DFS)
        public int DFS(int u, int v)
        {
            bool[] visited = new bool[V];
            int[] dist = new int[V];

            for (int i = 0; i < V; i++)
            {
                visited[i] = false;
                dist[i] = -1;
            }

            visited[u] = true;
            dist[u] = 0;

            Stack<int> stack = new Stack<int>();
            stack.Push(u);

            while (stack.Count != 0)
            {
                int s = stack.Pop();

                foreach (int i in adj[s])
                {
                    if (!visited[i])
                    {
                        visited[i] = true;
                        dist[i] = dist[s] + 1;
                        stack.Push(i);

                        // اگر به دنبال گره v باشید ، دوره را برمی گرداند
                        if (i == v)
                        {
                            return dist[v];
                        }
                    }
                }
            }

            return -1; // اگر v را نتوان برای رسیدن پیدا کرد
        }
    }

    class dfa
    {
        public string states { get; set; }

        public string input_symbols { get; set; }


        public Dictionary<string, Dictionary<string, string>> transitions { get; set; }


        public string initial_state { get; set; }

        public string final_states { get; set; }

        public static main_dfa converter(dfa tmp)
        {
            char a = "'".ToCharArray()[0];
            char[] spliter = new char[] { ',', a, '}', '{' };
            //states
            List<string> file_states = tmp.states.Split(spliter).ToList();
            List<string> final_states = new List<string>();
            for (int i = 0; i < file_states.Count; i++)
            {
                if (file_states[i] != "")
                    final_states.Add(file_states[i]);
            }

            //input_symbols
            List<string> file_input = tmp.input_symbols.Split(spliter).ToList();
            List<string> final_input = new List<string>();
            for (int i = 0; i < file_input.Count; i++)
            {
                if (file_input[i] != "")
                    final_input.Add(file_input[i]);
            }



            //transitions

            Dictionary<string, Dictionary<string, string>> transitions_final = tmp.transitions;

            // initial state
            string initial_final = tmp.initial_state;

            //final states

            List<string> file_finalstates = tmp.final_states.Split(spliter).ToList();
            List<string> final_finalstates = new List<string>();
            for (int i = 0; i < file_finalstates.Count; i++)
            {
                if (file_finalstates[i] != "")
                    final_finalstates.Add(file_finalstates[i]);
            }
            main_dfa set = new main_dfa();
            set.final_states = final_finalstates;
            set.initial_state = initial_final;
            set.transitions = transitions_final;
            set.input_symbols = final_input;
            set.states = final_states;

            return set;

        }

    }


    class main_dfa
    {
        public List<string> states { get; set; }

        public List<string> input_symbols { get; set; }

        public Dictionary<string, Dictionary<string, string>> transitions { get; set; }


        public string initial_state { get; set; }

        public List<string> final_states { get; set; }


        public List<string> non_final { get; set; }

        public bool[] reachable { get; set; }

        public List<int>[] adjancy = new List<int>[] { };
        public void set_nonfinal()
        {
            non_final = new List<string>();
            for (int i = 0; i < states.Count; i++)
            {
                if (!final_states.Contains(states[i]))
                    if (states[i] != null)
                        non_final.Add(states[i]);

            }
            foreach (string s in non_final)
                Console.WriteLine(s);
        }

        public void setadjancylist()
        {

            adjancy = new List<int>[states.Count];
            for (int i = 0; i < adjancy.Length; i++)
                adjancy[i] = new List<int> { };
            foreach (var state in transitions.OrderBy(x => x.Key))
                foreach (var adj in state.Value)
                {
                    char[] placer = state.Key.ToCharArray();
                    int place = int.Parse(placer[1].ToString());
                    adjancy[place].Add(int.Parse(adj.Value.ToCharArray()[1].ToString()));
                }

            for (int i = 0; i < adjancy.Length; i++)
            {
                foreach (var adj in adjancy[i])
                    Console.Write(adj + " ");
                Console.WriteLine();
            }

        }
        public void reach()
        {
            reachable = new bool[states.Count];
            for (int i = 0; i < reachable.Length; i++)
            {
                reachable[i] = true;
            }
        }
    }


    class program
    {
        static dfa json_Dictionary;
        public static void LoadJson()
        {
            string jsonFilePath = @".\samples\phase2-sample\in\input1.json";

            string json = File.ReadAllText(jsonFilePath);
            json_Dictionary = JsonConvert.DeserializeObject<dfa>(json);

            main_dfa set = dfa.converter(json_Dictionary);


            set.setadjancylist();

            set.reach();

            int init;

            init = int.Parse(set.initial_state.ToCharArray()[1].ToString());


            //Console.WriteLine(init);

            Graph g = new Graph(set.states.Count);
            g.adj = set.adjancy;
            for (int i = 0; i < set.states.Count; i++)
            {
                if (g.DFS(int.Parse(set.initial_state.ToCharArray()[1].ToString()),
                    int.Parse(set.states[i].ToCharArray()[1].ToString())) == -1)
                {
                    if (set.states[i] != set.initial_state)
                    {
                        set.states[i] = null;
                        set.reachable[i] = false;
                    }
                }
                //Console.WriteLine(set.states[i]);
            }
            set.set_nonfinal();

            set.non_final.OrderBy(x => x).ToList();
            string[,] table = new string[set.states.Count, set.input_symbols.Count + 1];
            for (int i = 0; i < set.non_final.Count; i++)
            {
                table[i, 0] = set.non_final[i];
                table[i, 1] = "q" + set.adjancy[i].ElementAt(0).ToString();
                table[i, 2] = "q" + set.adjancy[i].ElementAt(1).ToString();
                Console.WriteLine(table[i, 0] + " " + table[i, 1] + " " + table[i, 2]);
            }


            for (int i = 0; i < set.states.Count - set.non_final.Count; i++)
            {
                int hold = set.non_final.Count;
                table[i + hold, 0] = set.final_states[i];
                int place = int.Parse(set.final_states[i].ToCharArray()[1].ToString());
                table[i + hold, 1] = "q" + set.adjancy[place].ElementAt(0).ToString();
                table[i + hold, 2] = "q" + set.adjancy[place].ElementAt(1).ToString();
                Console.WriteLine(table[i + hold, 0] + " " + table[i + hold, 1] + " " + table[i + hold, 2]);
            }


            //copy
            string[,] copytable = new string[set.states.Count, set.input_symbols.Count + 1]; copytable = table.Clone() as string[,];


            ////////////////////////////////
            string[] copyfinal = new string[set.final_states.Count];
            set.final_states.CopyTo(copyfinal);
            List<string> final_state = copyfinal.ToList();

            string[] copystate = new string[set.states.Count];
            set.states.CopyTo(copystate);
            List<string> states = set.states;

            for (int i = 0; i < table.GetLength(0); i++)
            {
                if (set.final_states.Contains(table[i, 0]))
                {
                    int index = set.final_states.IndexOf(table[i, 0]);
                    //   set.final_states[index] = "g" + "01";
                    set.states[i] = "g" + "01";
                    table[i, 0] = "g" + "01";


                }
                else
                {
                    set.states[i] = "g" + "00";
                    table[i, 0] = "g" + "00";
                }
                //transition 0
                if (final_state.Contains(table[i, 1]))
                {

                    table[i, 1] = "g" + "01";


                }
                else
                {

                    table[i, 1] = "g" + "00";
                }
                //transition 1
                if (final_state.Contains(table[i, 2]))
                {

                    table[i, 2] = "g" + "01";


                }
                else
                {

                    table[i, 2] = "g" + "00";
                }
                Console.WriteLine(table[i, 0] + " " + table[i, 1] + " " + table[i, 2]);
            }
            minimize(set);



        }
        static Dictionary<string, string> groupKinds = new Dictionary<string, string>();
        static Dictionary<string, string> previous_grouping = new Dictionary<string, string>();
        static Dictionary<string, string> new_grouping = new Dictionary<string, string>();
        static Dictionary<string, Dictionary<string, string>> finalAnswerDict = new Dictionary<string, Dictionary<string, string>>();
        static string initialstate = "";
        static List<string> finalStates = new List<string>();
        static int stage = 1;
        static int totalGroupCount = -1;
        static void minimize(main_dfa dfa)
        {
            for (int i = 0; i < dfa.non_final.Count; i++)
            {
                previous_grouping.Add(dfa.non_final[i], "g00");
            }
            for (int i = 0; i < dfa.final_states.Count; i++)
            {
                previous_grouping.Add(dfa.final_states[i], "g01");
            }
            while (true)
            {
                foreach (string state in previous_grouping.Keys)
                {
                    string label = transitionsLabel(dfa, state);

                    if (groupKinds.Keys.Contains(label))
                    {
                        new_grouping.Add(state, groupKinds[label]);
                    }
                    else
                    {
                        groupKinds.Add(label, "g" + stage.ToString() +( groupKinds.Count + 1));
                        new_grouping.Add(state, groupKinds[label]);
                    }
                }
                if (groupKinds.Count == totalGroupCount)
                {
                    break;
                }
                previous_grouping = new_grouping;
                new_grouping = new Dictionary<string, string>();
                totalGroupCount = groupKinds.Count;
                groupKinds = new Dictionary<string, string>();
            }
            Grouping();
            initialstate = previous_grouping[dfa.initial_state];
            foreach (string item in dfa.final_states)
            {
                if (!finalStates.Contains(previous_grouping[item]))
                {
                    finalStates.Add(previous_grouping[item]);
                }
            }

        }
        static void Grouping()
        {
            foreach (string item in groupKinds.Keys)
            {
                string state = groupKinds[item];
                string[] transitions = item.Split("-");
                Dictionary<string, string> group = new Dictionary<string, string>();
                for (int i = 0; i < transitions.Length - 1; i += 2)
                {
                    group.Add(transitions[i], transitions[i + 1]);
                }
                finalAnswerDict.Add(state, group);
            }
        }
        static void PrintFinalAnswer()
        {

        }
        static string transitionsLabel(main_dfa dfa, string state)
        {
            List<string> label = new List<string>();
            for (int i = 0; i < dfa.input_symbols.Count; i++)
            {
                string ssde = previous_grouping[dfa.transitions[state][dfa.input_symbols[i]]];
             //   label += (dfa.input_symbols[i] + "-" + ssde);
                label.Add(dfa.input_symbols[i]);
                label.Add(ssde);
            }
            //    Console.WriteLine(label);
            string answer = String.Join("-", label);
            Console.WriteLine(answer);
            return answer;
        }

        static void Main()
        {
            LoadJson();

            phase4.program.main();



            //Console.ReadKey();


            
            dfa final = new dfa();

            //states
            string f_state = "";

            f_state += "{";
            List<string> f_states = finalAnswerDict.Keys.ToList();
            foreach (string s in f_states) f_state += "'" + s + "'" + ",";
            char[] help = f_state.ToCharArray();
            f_state = null;
            for (int i = 0; i < help.Length - 1; i++)
                f_state += help[i];
            f_state += "}";
            final.states = f_state;
            //input symbols
            Console.WriteLine(f_state);
            final.input_symbols = json_Dictionary.input_symbols;

            //transitions
            foreach(string item in finalAnswerDict.Keys)
            {
                Console.WriteLine("\n"+item);
                foreach(string state in finalAnswerDict[item].Keys)
                {
                    Console.Write(state+" "+finalAnswerDict[item][state]+" ");
                    
                }
            }
            final.transitions = finalAnswerDict;

            //initial state

            final.initial_state = initialstate;

            //final states

            f_state = "{";
            f_states = finalStates;
            foreach (string s in f_states) f_state += "'" + s + "'" + ",";
            help = f_state.ToCharArray();
            f_state = null;
            for (int i = 0; i < help.Length - 1; i++)
                f_state += help[i];
            f_state += "}";
            final.final_states = f_state;




            string mtn = JsonConvert.SerializeObject(final);
            File.WriteAllText(@".\phase2.json", mtn);
            Console.WriteLine(mtn);
        }
    }
}
