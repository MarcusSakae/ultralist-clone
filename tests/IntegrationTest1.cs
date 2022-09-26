using Xunit;
using Cheap.Ultralist.KnockOff;
using System.Text.Json;
using Xunit.Sdk;
using System.Reflection;
using System.Diagnostics;

namespace ultralist_clone_test
{

    public class RunBefore : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            File.Delete("tasks.json");
        }
    }

    public class IntegrationTest1
    {

        [Fact(DisplayName = "'ul' without any arguments should show help")]
        [RunBefore]
        public void NoArgs_ShowsHelp()
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Program.Main(new string[0]);
            Assert.Contains("Ul, cheap ultralist knockoff", sw.ToString());
            Assert.Contains("Add a task", sw.ToString());
            Assert.Contains("List all tasks", sw.ToString());
        }

        [Fact(DisplayName = "'ul help' should show help")]
        public void Help_ShowsHelp()
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Program.Main(new string[] { "help" });
            Assert.Contains("Ul, cheap ultralist knockoff", sw.ToString());
            Assert.Contains("Add a task", sw.ToString());
            Assert.Contains("List all tasks", sw.ToString());
        }

        [Fact(DisplayName = "'ul init' should create a tasks.json file")]
        [RunBefore]
        public void Init_CreatesFile()
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Program.Main(new string[] { "init" });
            Assert.Contains(" created in this folder!", sw.ToString());
            Assert.True(File.Exists("tasks.json"));
        }

        [Fact(DisplayName = "'ul init' with a tasks.json present should show error")]
        [RunBefore]
        public void InitTwice_ShowsError()
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Program.Main(new string[] { "init" });
            Program.Main(new string[] { "init" });
            Assert.Contains("Error", sw.ToString());
        }

        [Fact(DisplayName = "'ul add' should add a task")]
        [RunBefore]
        public void AddTask_OutputsSuccess()
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Program.Main(new string[] { "init" });
            Program.Main(new string[] { "add", "something" });
            Assert.Contains("Task", sw.ToString());
            Assert.Contains("added.", sw.ToString());
            var tasks = JsonSerializer.Deserialize<List<Task>>(File.ReadAllText("tasks.json"))!;
            Assert.Single(tasks);
        }

        [Fact(DisplayName = "'ul a' should add a task")]
        [RunBefore]
        public void AddTaskUsingAlias_OutputsSuccess()
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Program.Main(new string[] { "init" });
            Program.Main(new string[] { "a", "something" });
            Assert.Contains("Task", sw.ToString());
            Assert.Contains("added.", sw.ToString());
            var tasks = JsonSerializer.Deserialize<List<Task>>(File.ReadAllText("tasks.json"))!;
            Assert.Single(tasks);
        }

        [Fact(DisplayName = "'ul edit' should edit a task")]
        [RunBefore]
        public void EditTask_UpdatesTask()
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Program.Main(new string[] { "init" });
            // Add and edit
            Program.Main(new string[] { "a", "something" });
            Program.Main(new string[] { "edit", "1", "something else" });
            var tasks = JsonSerializer.Deserialize<List<Task>>(File.ReadAllText("tasks.json"))!;
            Assert.Equal("something else", tasks[0].Text);
            // Edit using alias
            Program.Main(new string[] { "e", "1", "something else again" });
            tasks = JsonSerializer.Deserialize<List<Task>>(File.ReadAllText("tasks.json"))!;
            Assert.Equal("something else again", tasks[0].Text);
        }


        [Fact(DisplayName = "'ul addnote' without arguments should show error")]
        [RunBefore]
        public void AddNote_WithoutArgs_ShowsError()
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            Program.Main(new string[] { "init" });
            // Add and edit
            Program.Main(new string[] { "addnote"});
            Assert.Contains("Error", sw.ToString());
        }
    }
}

