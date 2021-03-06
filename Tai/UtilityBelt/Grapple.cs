﻿using System;
using System.Text;

namespace Tai.UtilityBelt {
	internal static class Grapple {

        internal static bool isAllowingHumanInteraction = true;
        ////								_ 1      1 __        _.xxxxxx.
        ////				 [xxxxxxxxxxxxxx|##|xxxxxxxx|##|xxxxxxXXXXXXXXX|
        //// ____            [XXXXXXXXXXXXXXXXXXXXX/.\||||||XXXXXXXXXXXXXXX|
        ////|::: `-------.-.__[=========---___/::::|::::::|::::||X O^XXXXXX|
        ////|::::::::::::|2|%%%%%%%%%%%%\::::::::::|::::::|::::||X /
        ////|::::,-------|_|~~~~~~~~~~~~~`---=====-------------':||  5
        //// ~~~~                       |===|:::::|::::::::|::====:\O
        ////							|===|:::::|:.----.:|:||::||:|
        ////							|=3=|::4::`'::::::`':||__||:|
        ////							|===|:::::::/  ))\:::`----':/
        ////BlasTech Industries'        `===|::::::|  // //~`######b
        ////DL-44 Heavy Blaster Pistol      \`--------=====/  ######B
        ////												  `######b
        ////1 .......... Sight Adjustments                    #######b
        ////2 ............... Stun Setting                    #######B
        ////3 ........... Air Cooling Vent                    `#######b
        ////4 ................. Power Pack                     #######P
        ////5 ... Power Pack Release Lever             LS      `#####B

        internal static string GetThisFolder() {
            var path_bits = System.Reflection.Assembly.GetEntryAssembly().Location.Split('\\');

            string simple_path_i_have_to_build_because_you_suck_microsoft = "";

            for(int i=0; i<(path_bits.Length-1); ++i) {
                simple_path_i_have_to_build_because_you_suck_microsoft += path_bits[i]+'\\';
            }

            return simple_path_i_have_to_build_because_you_suck_microsoft;
        }

        internal static string GetUsernameFromTerminal() {
            TryShortCircuit("username");
	        //todo: validate email format regex ? /w.*\@/w.*\.com
	        Console.Write("enter username\nex: jbond@gmail.com\n\nyour username: ");
	        return Console.ReadLine();
        }

        internal static string GetPasswordFromTerminal() {
            TryShortCircuit("password");
	        //TODO: actually hide password
	        Console.Write("enter password\n\nyour hidden password: ");
	        return Console.ReadLine();
        }

        internal static string[] GetTeamMemberFirstNamesFromTerminal() {
            TryShortCircuit("statusReportNames");
	        Console.Write("enter first name(s) of your dev team members seperated by a single space \nex: ross derrick antonio \n\nfirst names: ");
	        return Console.ReadLine().Split(' ');
        }

        internal static TaiConfig TryGetCredentialsManually(TaiConfig conf) {
	        if(!conf.ContainsKey("username")) {
		        conf["username"] = GetUsernameFromTerminal();}

	        if(!conf.ContainsKey("password")) {
		        conf["password"] = GetPasswordFromTerminal();}
			
            return conf;
        }
        private static void TryShortCircuit(string requiredPropertyName) {
            if(!isAllowingHumanInteraction) {
                Console.WriteLine(string.Format("you did not provide property {0} in either the conf file or command args", requiredPropertyName));
                System.Environment.Exit(0);
            }
        }
	}
}
