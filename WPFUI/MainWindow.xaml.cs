﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Engine.EventArgs;
using Engine.ViewModels;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameSession _gameSession; // create private variable
        //Game session = View Model
        public MainWindow()
        {
            InitializeComponent();

            _gameSession = new GameSession(); //create new game session and assign it to the varible we created

            _gameSession.OnMessageRaised += OnGameMessageRaised;

            DataContext = _gameSession; // This will be the source of data for the databinding we do

        }

        private void OnClick_MoveNorth(Object sender, RoutedEventArgs e)
        {
            _gameSession.MoveNorth();
        }

        private void OnClick_MoveWest(Object sender, RoutedEventArgs e)
        {
            _gameSession.MoveWest();
        }

        private void OnClick_MoveEast(Object sender, RoutedEventArgs e)
        {
            _gameSession.MoveEast();
        }

        private void OnClick_MoveSouth(Object sender, RoutedEventArgs e)
        {
            _gameSession.MoveSouth();
        }

        // ViewModel can send messages, we need the View to watch for these messages – “subscribe” to the eventhandler.
        private void OnGameMessageRaised(Object sender, GameMessagesEventArgs e)
        {
            GameMessages.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            GameMessages.ScrollToEnd(); // user can always see the latest message
        }
    }
}