﻿using System;
using Mono.Options;
using Tai.Extensions;
using Tai.UtilityBelt;
using System.Collections.Generic;

namespace Tai {

    class Program {

        delegate void TaiMethod(TaiConfig config); //todo: delegate string TaiMethod(TaiConfig conf, OUTPUT_TYPE outType)

        private enum TAI_COMMAND {NONE, 
            ITERATION_REPORT, 
            BURNDOWN, 
            SET_BUILDID, 
            CREATE_TASK, 
            GET_ITERATION, 
            GET_MY_STORYS, 
            GET_TEAM_STORYS, 
            GET_TEAM_STORY_URLS};

        private static TAI_COMMAND SESSION_ACTION = TAI_COMMAND.NONE;

        private static Dictionary<TAI_COMMAND, TaiMethod> TaiTakeCareOfThis = new Dictionary<TAI_COMMAND, TaiMethod>() {
            {TAI_COMMAND.NONE, _ => {Echo.HelpText();}},
            {TAI_COMMAND.ITERATION_REPORT, CLIMethod.WriteStatusReportForAnIteration},
            {TAI_COMMAND.BURNDOWN, CLIMethod.AutomaticallyFillTaskTime},
            {TAI_COMMAND.SET_BUILDID, CLIMethod.SetStoryBuildId},
            {TAI_COMMAND.CREATE_TASK, CLIMethod.CreateTaskForStory},
            {TAI_COMMAND.GET_ITERATION, CLIMethod.GetCurrentIterationNumber},
            {TAI_COMMAND.GET_MY_STORYS, CLIMethod.GetStorysForUser},
            {TAI_COMMAND.GET_TEAM_STORYS, CLIMethod.GetTeamStoryIds},
            {TAI_COMMAND.GET_TEAM_STORY_URLS, CLIMethod.GetTeamStoryUrls},

        };

        public static void Main(string[] args) {
            
            TaiConfig config = new TaiConfig(@Grapple.GetThisFolder() + "tai.conf");

            var options = new OptionSet() {
                { "iteration-report", "", _ => {SESSION_ACTION = TAI_COMMAND.ITERATION_REPORT;}},
                { "auto-fill-time", "", _ => {SESSION_ACTION = TAI_COMMAND.BURNDOWN;}},
                { "set-story-build", "", _ => {SESSION_ACTION = TAI_COMMAND.SET_BUILDID;}},
                { "create-task", "", _ => {SESSION_ACTION = TAI_COMMAND.CREATE_TASK;}},
                { "get-iteration", "", _ => {SESSION_ACTION = TAI_COMMAND.GET_ITERATION;}},
                { "get-my-storys", "", _ => {SESSION_ACTION = TAI_COMMAND.GET_MY_STORYS;}},
                { "get-team-storys", "", _ => {SESSION_ACTION = TAI_COMMAND.GET_TEAM_STORYS;}},
                { "get-team-story-urls", "", _ => {SESSION_ACTION = TAI_COMMAND.GET_TEAM_STORY_URLS;}},



                { "?|h|help", "", _ => Echo.HelpText()},
                {"no-interaction", "", _ => {Grapple.isAllowingHumanInteraction = false;}},
                
                {"username=", "", user => {config["username"] = user;}},
                {"password=", "", pass => {config["password"] = pass;}},
                {"api-url=", "", url => {config["apiUrl"] = url;}}, 
                {"project-id=", "", proj => {config["projectId"] = proj;}},
                {"target-user=", "", user => {config["targetUser"] = user;}},
                {"story-id=", "", storyId => {config["storyId"] = storyId;}},
                {"build-id=", "", buildId => {config["buildId"] = buildId;}},
                {"burndown-date=", "", date => {config["burndownDate"] = date;}},
                {"email-greeting=", "", hi => {config["emailGreeting"] = hi;}},
                {"email-signature=", "", me => {config["emailSignature"] = me;}},
                {"iteration-number=", "", num => {config["iterationNumber"] = num;}},
                {"hours-per-day=", "", hours => {config["hoursPerDay"] = hours;}},
                {"description=", "", desc => {config["description"] = desc;}},
                {"note=|notes=", "", note => {config["notes"] = note;}},
                {"block=|blocked=", "", isblock => {config["isBlocked"] = isblock;}},
                {"estimate-hours=", "", hours => {config["estimateHours"] = hours;}},
                {"task-state=", "", state => {config["taskState"] = state;}}, /* "Defined", "In-Progress", "Completed" */
                {"task-name=", "", name => {config["taskName"] = name;}},
                {"attachment-type=", "", type => {config["attatchmentType"] = type;}}, //take in the mime-type. type == "base64" indicates that tai does not need to encode it

                {"status-report-names=", "", names => {config["statusReportNames"] = names.Split(',').ToJson();}},
                {"task-names=", "", names => {config["taskNames"] = names.Split(',').ToJson();}},
                {"attachment=|attachments=", "", files => {config["attachments"] = files.Split(',').ToJson();}}, //can be the path or raw file, but all types must be the same for this up and comming version

                {"d=|delimiter=", "", sep => {Echo.DELIMITER = sep;}},
                {"l=|log-level=", "", noise => {Echo.LOG_LEVEL = Convert.ToByte(noise);}},
                                
                /*
                later...
                get-stories
                get-tasks

                team-name=

                output-type= json csv none(same as log-level 0) cli(default)
                */
            };

            var badInput = options.Parse(args);

            if(badInput.Count > 0) {
                Echo.ErrorReport(badInput.ToArray());
                Echo.OffensiveGesture();
                Echo.HelpText();
            }

            Echo.WelcomeText();

            config = Grapple.TryGetCredentialsManually(config);
            ApiWrapper.Initialize(config);

            TaiTakeCareOfThis[SESSION_ACTION](config);

            Echo.Out("done", 5);
        }
    }
}