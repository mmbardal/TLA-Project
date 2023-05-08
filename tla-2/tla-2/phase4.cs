using System;
using System.Collections.Generic;
using System.Linq;


using Newtonsoft.Json;


namespace phase4
{
    class fa
    {
        public string states { get; set; }

        public string input_symbols { get; set; }


        public Dictionary<string, Dictionary<string, string>> transitions { get; set; }


        public string initial_state { get; set; }

        public string final_states { get; set; }

        public static main_fa converter(fa tmp)
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

            Dictionary<string, Dictionary<string, List<string>>> transitions_final = new Dictionary<string, Dictionary<string, List<string>>>();
           
            foreach (var item in tmp.transitions)
            {
                transitions_final.Add(item.Key, new Dictionary<string, List<string>>());
                foreach (var state in item.Value)
                {
                    List<string> helper = new List<string>();

                    string help = tmp.transitions[item.Key][state.Key];
                    List<string> trans = help.Split(spliter).ToList();
                    for (int j = 0; j < trans.Count; j++)
                        if (trans[j] != "") helper.Add(trans[j]);
                    transitions_final[item.Key].Add(state.Key, helper);
                }
            }
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
            main_fa set = new main_fa();
            set.final_states = final_finalstates;
            set.initial_state = initial_final;
            set.transitions = transitions_final;
            set.input_symbols = final_input;
            set.states = final_states;

            return set;

        }

    }



    class main_fa
    {
        public List<string> states { get; set; }

        public List<string> input_symbols { get; set; }

        public Dictionary<string, Dictionary<string, List<string>>> transitions { get; set; }


        public string initial_state { get; set; }

        public List<string> final_states { get; set; }


        public List<string> non_final { get; set; }

        public bool[] reachable { get; set; }

        public List<int>[] adjancy = new List<int>[] { };

        public fa converter(main_fa shit)
        {
            fa final = new fa();
            //states
            string f_state = "";

            f_state += "{";

            foreach (string s in shit.states) f_state += "'" + s + "'" + ",";
            char[] help = f_state.ToCharArray();
            f_state = null;
            for (int i = 0; i < help.Length - 1; i++)
                f_state += help[i];
            f_state += "}";

            final.states = f_state;
            f_state = null;
            // initial state
            final.initial_state = shit.initial_state;
            // input symbols 
            f_state += "{";

            foreach (string s in shit.input_symbols) f_state += "'" + s + "'" + ",";
            help = f_state.ToCharArray();
            f_state = null;
            for (int i = 0; i < help.Length - 1; i++)
                f_state += help[i];
            f_state += "}";

            final.input_symbols = f_state;
            f_state = null;

            // transition
            final.transitions = new Dictionary<string, Dictionary<string, string>>();
            
            foreach (var item in shit.transitions)
            {
                final.transitions.Add(item.Key, new Dictionary<string, string>());
                final.transitions[item.Key] = new Dictionary<string, string>();
                foreach (var itemx in item.Value)
                {
                    List<string> list = new List<string>();
                    list = itemx.Value;
                    f_state += "{";

                    foreach (string s in list) f_state += "'" + s + "'" + ",";
                    help = f_state.ToCharArray();
                    f_state = null;
                    for (int i = 0; i < help.Length - 1; i++)
                        f_state += help[i];
                    f_state += "}";
                    final.transitions[item.Key].Add(itemx.Key, f_state);
                    f_state = null;
                }
            }

            // final state

            f_state += "{";

            foreach (string s in shit.final_states) f_state += "'" + s + "'" + ",";
            help = f_state.ToCharArray();
            f_state = null;
            for (int i = 0; i < help.Length - 1; i++)
                f_state += help[i];
            f_state += "}";

            final.final_states = f_state;
            f_state = null;

            //return fa
            return final;


        }






    }

    class program
    {
        public static fa TEST;
        public static fa TEST1;
        public static fa TEST2;
        public static fa TEST3;
        public static fa TEST4;



        static main_fa concat(main_fa set1, main_fa set2)
        {
            main_fa final = new main_fa();
            //change name 
            for (int i = 0; i < set2.states.Count; i++)
                set2.states[i] += "x";
            for (int i = 0; i < set2.final_states.Count; i++)
                set2.final_states[i] += "x";
            set2.initial_state += "x"; 
            foreach (var item in set2.transitions)
                foreach (var itemx in item.Value)
                {
                    for (int i = 0; i < itemx.Value.Count; i++)
                    { itemx.Value[i] += "x"; }
                       
                }


            //state
            final.states = set1.states.Concat(set2.states).ToList();
            //input
            final.input_symbols = set1.input_symbols.Concat(set2.input_symbols).ToList();
            //initial state
            final.initial_state = set1.initial_state;
            final.final_states = set2.final_states;
            //transitions
            final.transitions = set1.transitions;
            foreach (var item in set2.transitions)
                final.transitions.Add(item.Key + "x", item.Value);
            //connect initial states

            for (int i = 0; i < set1.final_states.Count; i++)
            {
                if (!final.transitions[set1.final_states[i]].Keys.Contains(""))
                { final.transitions[set1.final_states[i]].Add("", new List<string>()); }
                final.transitions[set1.final_states[i]][""].Add(set2.initial_state);
            }
            return final;
        }

        static main_fa star(main_fa set)
        {
            string initial_key = set.initial_state;
            List<string> final_key = set.final_states;


            //initial to final
            if (set.transitions[initial_key].Keys.Contains(""))
            {
                for (int i = 0; i < final_key.Count; i++)
                    set.transitions[initial_key][""].Add(final_key[i]);
            }
            else
            {
                set.transitions[initial_key].Add("", new List<string>());
                for (int i = 0; i < final_key.Count; i++)
                    set.transitions[initial_key][""].Add(final_key[i]);
            }

            //final to initial
            for (int i = 0; i < final_key.Count; i++)
            {
                if (set.transitions[final_key[i]].Keys.Contains(""))
                {

                    set.transitions[final_key[i]][""].Add(initial_key);
                }
                else
                {
                    set.transitions[final_key[i]].Add("", new List<string>());

                    set.transitions[final_key[i]][""].Add(initial_key);
                }
            }

            return set;

        }

        static main_fa union(main_fa set1, main_fa set2)
        {
            main_fa final = new main_fa();
            //change name 
            for (int i = 0; i < set2.states.Count; i++)
                set2.states[i] += "x";
            for (int i = 0; i < set2.final_states.Count; i++)
                set2.final_states[i] += "x";
            set2.initial_state += "x";
            foreach (var item in set2.transitions)
                foreach (var itemx in item.Value)
                {
                    for (int i = 0; i < itemx.Value.Count; i++)
                    { itemx.Value[i] += "x"; }
                }


            //state
            final.states = set1.states.Concat(set2.states).ToList();
            //input
            final.input_symbols = set1.input_symbols.Concat(set2.input_symbols).ToList();
            final.initial_state = "start";
            final.final_states = set1.final_states.Concat(set2.final_states).ToList();
            //transitions
            final.transitions = set1.transitions;
            foreach (var item in set2.transitions)
                final.transitions.Add(item.Key + "x", item.Value);
            //connect initial states
            final.transitions.Add("start", new Dictionary<string, List<string>>());
            final.transitions["start"].Add("", new List<string>());
            final.transitions["start"][""].Add(set1.initial_state);
            final.transitions["start"][""].Add(set2.initial_state);


            return final;
        }

        public static void main()
        {
            //for star
            string jsonFilePath1 = @".\samples\phase4-sample\star\in\FA.json";
            //for concat
            string jsonFilePath2 = @".\samples\phase4-sample\concat\in\FA1.json";
            string jsonFilePath3 = @".\samples\phase4-sample\concat\in\FA2.json";
            //for union 
            string jsonFilePath4 = @".\samples\phase4-sample\union\in\FA1.json";
            string jsonFilePath5 = @".\samples\phase4-sample\union\in\FA2.json";

            //loading
            string json = File.ReadAllText(jsonFilePath1);
            string json2 = File.ReadAllText(jsonFilePath2);
            string json3 = File.ReadAllText(jsonFilePath3);
            string json4 = File.ReadAllText(jsonFilePath4);
            string json5 = File.ReadAllText(jsonFilePath5);
            TEST = JsonConvert.DeserializeObject<fa>(json);
            TEST1 = JsonConvert.DeserializeObject<fa>(json4);
            TEST2 = JsonConvert.DeserializeObject<fa>(json5);
            TEST3 = JsonConvert.DeserializeObject<fa>(json2);
            TEST4 = JsonConvert.DeserializeObject<fa>(json3);

            //star
            main_fa set = fa.converter(TEST);
            main_fa star_f = star(set);
            fa star_x = star_f.converter(star_f);
            string mtn = JsonConvert.SerializeObject(star_x);
            File.WriteAllText(@".\star.json", mtn);
            Console.WriteLine(mtn);

            mtn = null;

            //union
            main_fa set1 = fa.converter(TEST1);
            main_fa set2 = fa.converter(TEST2);
            main_fa union_f = union(set1, set2);
            fa union_x = union_f.converter(union_f);

            mtn = JsonConvert.SerializeObject(union_x);
            File.WriteAllText(@".\union.json", mtn);
            Console.WriteLine(mtn);
            mtn = null;
            //concat
            main_fa union1 = fa.converter(TEST3);
            main_fa union2 = fa.converter(TEST4);
            main_fa concat_f = concat(union1, union2);
            fa concat_x = concat_f.converter(concat_f);


            mtn = JsonConvert.SerializeObject(concat_x);
            File.WriteAllText(@".\concat.json", mtn);
            Console.WriteLine(mtn);
            mtn = null;

        }
    }
}