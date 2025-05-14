using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace EmotionEditor
{
    public partial class MainWindow : Window
    {
        // Словари с ключевыми словами для эмоций
        private List<string> joyKeywords = new List<string> { "счастье", "радость", "веселье", "смех", "удовлетворение" };
        private List<string> sadnessKeywords = new List<string> { "грусть", "печаль", "тоска", "плакать", "разочарование" };
        private List<string> angerKeywords = new List<string> { "злость", "гнев", "ярость", "негодование", "раздражение" };

        private DispatcherTimer timer;
        private Color currentColor = Colors.White;
        private Color targetColor = Colors.White;
        private double step = 0.1;

        public MainWindow()
        {
            InitializeComponent();

            // Таймер для плавного изменения фона
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += Timer_Tick;
        }

        private void TextEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = TextEditor.Text.ToLower();
            string emotion = AnalyzeEmotion(text);

            // Задаем анимацию фона в зависимости от эмоции
            ChangeBackgroundColor(emotion);
        }

        // Функция для анализа эмоции по ключевым словам
        private string AnalyzeEmotion(string text)
        {
            if (joyKeywords.Any(word => text.Contains(word))) return "joy";
            if (sadnessKeywords.Any(word => text.Contains(word))) return "sadness";
            if (angerKeywords.Any(word => text.Contains(word))) return "anger";

            return "neutral";  // Если нет ключевых слов, оставляем нейтральный фон
        }

        // Функция для изменения фона с анимацией
        private void ChangeBackgroundColor(string emotion)
        {
            targetColor = emotion switch
            {
                "joy" => Colors.Yellow,
                "sadness" => Colors.LightBlue,
                "anger" => Colors.Red,
                _ => Colors.White
            };

            // Стартуем плавное изменение фона
            if (!timer.IsEnabled)  // Чтобы избежать множественного запуска таймера
                timer.Start();
        }

        // Обработчик таймера для плавного изменения фона
        private void Timer_Tick(object? sender, EventArgs e)  // Обработчик с учетом Nullable
        {
            currentColor = LerpColor(currentColor, targetColor, step);

            // Меняем фон всего окна
            this.Background = new SolidColorBrush(currentColor);

            // Останавливаем таймер, если цвет достиг цели
            if (Math.Abs(currentColor.R - targetColor.R) < 0.01 &&
                Math.Abs(currentColor.G - targetColor.G) < 0.01 &&
                Math.Abs(currentColor.B - targetColor.B) < 0.01)
            {
                timer.Stop();
            }
        }

        // Функция для линейной интерполяции между двумя цветами
        private Color LerpColor(Color from, Color to, double t)
        {
            byte r = (byte)(from.R + (to.R - from.R) * t);
            byte g = (byte)(from.G + (to.G - from.G) * t);
            byte b = (byte)(from.B + (to.B - from.B) * t);
            return Color.FromArgb(255, r, g, b);
        }
    }
}
