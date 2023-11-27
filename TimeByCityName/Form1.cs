using Newtonsoft.Json;
using Timer = System.Windows.Forms.Timer;

namespace TimeByCityName
{
    public partial class Form1 : Form
    {
        public class MyTimeZone
        {
            public string tZCode { get; set; }
            public string tZDesc { get; set; }
        }
        private TextBox cityTextBox;
        private Label timeLabel;
        private Timer timer;

        private List<MyTimeZone> timeZones;

        private void LoadTimeZones()
        {
            var json = File.ReadAllText("db.json");
            timeZones = JsonConvert.DeserializeObject<List<MyTimeZone>>(json);
        }
        public Form1()
        {
            InitializeComponent();
            LoadTimeZones();

            cityTextBox = new TextBox();
            cityTextBox.Location = new Point(30, 30);
            Controls.Add(cityTextBox);

            timeLabel = new Label();
            timeLabel.Width = 400;
            timeLabel.Location = new Point(30, 60);
            Controls.Add(timeLabel);

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += TimerHandler;
            timer.Start();
        }
        private void TimerHandler(object sender, EventArgs e)
        {
            var city = cityTextBox.Text;
            var timeZone = timeZones.FirstOrDefault(tz => tz.tZDesc.Contains(city));
            if (timeZone != null)
            {
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone.tZCode);
                var now = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                timeLabel.Text = $"Current time in {city} is {now}";
            }
            else
            {
                timeLabel.Text = $"Time zone for {city} not found";
            }
        }
    }
}