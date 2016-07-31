using System;

namespace psuedosense
{
	class MainClass
	{
		public static int verbose_level = 1;
		public static string[] var_table = new string[] {};
		public static string[] symtable = new string[4] {"IS","SAME","AS","SAY"};
		public static bool error = false;
		public string command = "None";
		public static void checksym(string identifer) {
			bool included = false;
			foreach (var symbol in var_table) {
				if (identifer == symbol.Split(':')[0]) {
					included = true;
				}
			}
			if (included == false) {
				string var_table_string = String.Join (" ", var_table);
				var_table_string += " ";
				var_table_string += identifer;
				var_table_string += ":NoValue";
				if (verbose_level == 3) {
					Console.Write ("[COMPUTER VAR TABLE] : ");
					Console.WriteLine (var_table_string);
				};
				var_table = var_table_string.Split (' ');
			}
			
		}
		public static bool check_assign(string identifier) {
			bool assigned = false;
			foreach (var symbol in var_table) {
				if (symbol.Split (':') [0] == identifier) {
					if (symbol.Split (':') [1] != "NoValue") {
						assigned = true;
					}
				}
			}
			return assigned;
		}
		public static void assign (string identifer){

			for (int i = 0; i <  var_table.Length; i++) {
				if (identifer == var_table[i].Split(':')[0]) {
					var_table[i] = var_table[i].Split(':')[0];
					var_table[i] += ":Assigned";
				}

			}
			string var_table_string = String.Join (" ", var_table);
			if (verbose_level == 3) {
				Console.Write ("[COMPUTER VAR TABLE] : ");
				Console.WriteLine (var_table_string);
			}
		}

		public static string[] lexical (string command){
			//uses symbol table to create tokens
			if (verbose_level == 3) {
				Console.WriteLine ("[COMPUTER LEXICAL ANALYSIS] : Lexical Analysis Started");
			}
			string[] words = command.Split(' ');
			string word_string = "";
			foreach (var word in words) {
				if (word.ToLower () != "the") {
					word_string += word;
					word_string += " ";
				}
			}
			words = word_string.Split (' ');
			string type = "start";
			foreach (var word in words) {
				if (word.ToLower () == "is") {
					type += " assignment";
				} else if (word.ToLower () == "as") {
					type += " comparison";
				} else if (word.ToLower () == "same") {
					//do a little dance
				} else if (word.ToLower () == "say") {
					type += " output";
				} else if (word.ToLower () != word) {
					type += " identifier";
					checksym (word);
				} else if (word == "") {
					// do a smaller dance
				} else {
					type += " value";
				}
			}
			if (verbose_level == 3) {
				Console.Write ("[COMPUTER LEXICAL ANALYSIS] : ");
				Console.WriteLine (type);
				Console.WriteLine ("[COMPUTER LEXICAL ANALYSIS] : Lexical Analysis Finished");
			}
			string[] tokens = type.Split (' ');
			return tokens;
		}
		public static void syntax (string[] tokens,string command){
			// check is token match up and creates var_table
			if (verbose_level == 3) {
				Console.WriteLine ("[COMPUTER SYNTAX ANALYSIS] : Syntax Analysis Started");
				Console.Write ("[COMPUTER SYNTAX ANALYSIS] : #");
				Console.Write (String.Join (" ", tokens));
				Console.WriteLine ("#");
			}
			string[] words = command.Split(' ');
			string word_string = "";
			foreach (var word in words) {
				if (word.ToLower () != "the") {
					word_string += word;
					word_string += " ";
				}
			}
			words = word_string.Split (' ');
			if (String.Join (" ", tokens) == "start identifier assignment value") {
				if (verbose_level == 3) {
					Console.WriteLine ("[COMPUTER SYNTAX ANALYSIS] : Assignment operation");
				}
				assign (words [0]);
			} else if (String.Join (" ", tokens) == "start assignment identifier value") {
				if (verbose_level == 3) {
					Console.WriteLine ("[COMPUTER SYNTAX ANALYSIS] : Comparison operation");
				}
				if (check_assign (words [1]) == false) {
					Console.Write ("[COMPUTER SYNTAX ERROR] : ");
					Console.Write (words [1]);
					Console.WriteLine (" is not defined");
					error = true;
				}
			} else if (String.Join (" ", tokens) == "start assignment identifier identifier") {
				if (verbose_level == 3) {
					Console.WriteLine ("[COMPUTER SYNTAX ANALYSIS] : Variable Comparison operation");
				}
				if (check_assign (words [1]) == false) {
					Console.Write ("[COMPUTER SYNTAX ERROR] : ");
					Console.Write (words [1]);
					Console.WriteLine (" is not defined");
					error = true;
				} else if (check_assign (words [2]) == false) {
					Console.Write ("[COMPUTER SYNTAX ERROR] : ");
					Console.Write (words [2]);
					Console.WriteLine (" is not defined");
					error = true;
				}
			}
			if (verbose_level == 3) {
				Console.WriteLine ("[COMPUTER SYNTAX ANALYSIS] : Syntax Analysis Finished");
			}
		}
		public static void compile (string[] tokens,string command){
			if (verbose_level > 1) {
				Console.WriteLine ("[COMPUTER COMPILER] : Running Command");
			}
			if (verbose_level == 3) {
				Console.Write ("[COMPUTER COMPILER] :");
				Console.Write (String.Join (" ", tokens));
			}
			string[] words = command.Split(' ');
			string word_string = "";
			foreach (var word in words) {
				if (word.ToLower () != "the") {
					word_string += word;
					word_string += " ";
				}
			}
			words = word_string.Split (' ');
			if (String.Join (" ", tokens) == "start identifier assignment value") {
				if (verbose_level > 1) {
					Console.WriteLine ("[COMPUTER COMPILER] : Assignment operation");
				}
				// assign value to variable 
				for (int i = 0; i <  var_table.Length; i++) {
					if (words[0] == var_table[i].Split(':')[0]) {
						var_table[i] = var_table[i].Split(':')[0];
						var_table[i] += ":";
						var_table[i] += words[2];
					}
				}
				string var_table_string = String.Join (" ", var_table);
				if (verbose_level == 3) {
					Console.Write ("[COMPUTER VAR TABLE] : ");
					Console.WriteLine (var_table_string);
				}
			} else if (String.Join (" ", tokens) == "start assignment identifier value") {
				if (verbose_level > 1) {
					Console.WriteLine ("[COMPUTER COMPILER] : Comparison operation");
				}
				// check if value is same as identifier
				bool same = false;
				for (int i = 0; i <  var_table.Length; i++) {
					if (words[1] == var_table[i].Split(':')[0]) {
						if (var_table [i].Split (':') [1] == words [2]) {
							same = true;
						}
					} 
				}
				Console.Write ("[COMPUTER OUTPUT] : ");
				if (same == true) {
					Console.WriteLine ("Yes");
				} else {
					Console.WriteLine ("No");
				}
			} else if (String.Join (" ", tokens) == "start assignment identifier identifier") {
				if (verbose_level > 1) {
					Console.WriteLine ("[COMPUTER COMPILER] : Variable Comparison operation");
				}
				// check if values of variables are same
				string value_one = "";
				for (int i = 0; i <  var_table.Length; i++) {
					if (words[1] == var_table[i].Split(':')[0]) {
						value_one = var_table [i].Split (':') [1];
					} 
				}
				string value_two = "";
				for (int i = 0; i <  var_table.Length; i++) {
					if (words[2] == var_table[i].Split(':')[0]) {
						value_two = var_table [i].Split (':') [1];
					} 
				}
				Console.Write ("[COMPUTER OUTPUT] : ");
				if (value_one == value_two) {
					Console.WriteLine ("Yes");
				} else {
					Console.WriteLine ("No");
				}
			}
			if (verbose_level == 3) {
				Console.WriteLine ("[COMPUTER COMPILER] : Command Finished");
			}
		}
		public static void Main (string[] args)
		{
			Console.WriteLine ("ENTER COMMANDS THEN FINISH TO END SESSION");
			String command = "hello";
			while (command.ToLower() != "finish") {
				Console.Write ("[YOU] : ");
				command = Console.ReadLine ();
				//special verbose case
				if (command.Split (' ') [0].ToLower () == "verbose") {
					if (command.Split (' ') [1].ToLower () == "low") {
						Console.WriteLine ("verbose level changed to low");
						verbose_level = 1;
					} else if (command.Split (' ') [1].ToLower () == "medium") {
						Console.WriteLine ("verbose level changed to meduium");
						verbose_level = 2;
					} else if (command.Split (' ') [1].ToLower () == "high") {
						Console.WriteLine ("verbose level changed to high");
						verbose_level = 3;
					}
				} else {
					string[] tokens = lexical (command);
					syntax (tokens, command);
					if (error == false) {
						compile (tokens, command);
					}
				}
			}
		}
	}
}