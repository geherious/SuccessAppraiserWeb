using Mono.TextTemplating;
using Serilog;
using SuccessAppraiserWeb.Areas.Goal.models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SuccessAppraiserWeb.Data.Goal.Initialize.Templates
{
    public class TemplateWithStatesInitializer
    {
        public static (GoalTemplate template, List<GoalState> states) Create(string templateName, Dictionary<string, string> stateParams)
        {
            GoalTemplate template = new GoalTemplate()
            {
                Name = templateName,
            };

            List<GoalState> states = new List<GoalState>();

            foreach (KeyValuePair<string, string> kvp in stateParams)
            {
                GoalState state = new GoalState()
                {
                    Name = kvp.Key,
                    Color = kvp.Value,
                };
                states.Add(state);
            }
            template.States.AddRange(states);
            return (template, states);
        }
        public static void Initialize(ApplicationDbContext context)
        {
            try
            {
                using (StreamReader stream = new StreamReader(@"Data\Goal\Initialize\Templates\StandartTemplatesWithStates.json"))
            {
                string json = stream.ReadToEnd();
                List<TemplateWithStates>? templateWithStates = JsonSerializer.Deserialize<List<TemplateWithStates>>(json);
                if (templateWithStates != null)
                {
                        foreach (TemplateWithStates tws in templateWithStates)
                        {
                            if (!context.GoalTemplates.Where(e => e.Name == tws.Name).Any())
                            {
                                GoalTemplate template = new GoalTemplate() { Name = tws.Name };
                                List<GoalState> goalStates = new List<GoalState>();
                                foreach (KeyValuePair<string, string> keyValue in tws.States)
                                {
                                    GoalState state = new GoalState();
                                    state.Name = keyValue.Key;
                                    state.Color = keyValue.Value;
                                    goalStates.Add(state);

                                }
                                template.States.AddRange(goalStates);
                                context.GoalTemplates.Add(template);
                                context.GoalStates.AddRange(goalStates);
                            }
                        }
                }
            }

            } catch (Exception ex)
            {
                if (ex is DirectoryNotFoundException ||  ex is FileNotFoundException)
                {
                    Log.Warning(ex, "Cannot find json file or directory for template initialization\n It should be located at Data\\Goal\\Initialize\\Templates\\");

                }
            }
        }
    }
}
