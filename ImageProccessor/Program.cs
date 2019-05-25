using System;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;
using static System.Console;

namespace AudioPlayer
{
    class Program
    {
        private static Player _player;
        private static Skin _skin;
        static void Main(string[] args)
        {
            _skin = new ClassicSkin();
            _skin.Render("Выберете стиль текста");
            _skin.Render("1. Классический стиль");
            _skin.Render("2. Цветной стиль");
            _skin.Render("3. Рандомный цвет стиль");
            WriteLine();
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                    {
                        _player = new Player(new ClassicSkin());
                        _skin = Player.Skin;
                        break;
                    }
                case ConsoleKey.NumPad1:
                    {
                        _player = new Player(new ClassicSkin());
                        _skin = Player.Skin;
                        break;
                    }

                case ConsoleKey.D2:
                    {
                        WriteLine();
                        _player = new Player(new ColorSkin(ChooseColor()));
                        _skin = Player.Skin;
                        break;
                    }
                case ConsoleKey.NumPad2:
                    {
                        WriteLine();
                        _player = new Player(new ColorSkin(ChooseColor()));
                        _skin = Player.Skin;
                        break;
                    }
                case ConsoleKey.D3:
                    {
                        _player = new Player(new RandomSkin());
                        _skin = Player.Skin;
                        break;
                    }
                case ConsoleKey.NumPad3:
                    {
                        _player = new Player(new RandomSkin());
                        _skin = Player.Skin;
                        break;
                    }
            }
            _player.PlayerStartedEvent += (sender, e) => { _skin.Render(e.Message); };
            _player.PlayerStoppedEvent += (sender, e) => { _skin.Render(e.Message); };
            _player.SongStarted += (sender, e) => { _skin.Render(e.Message); };
            _player.SongListChanged += (sender, e) => { _skin.Render(e.Message); };
            //_player.VolumeStatus += (sender, e) => { _skin.Render(e.Message); };
            //_player.LockOffEvent += (sender, e) => { _skin.Render(e.Message); };
            //_player.LockOnEvent += (sender, e) => { _skin.Render(e.Message); };
            _player.PlaylistLoadingEvent += (sender, e) => { _skin.Render(e.Message); };
            _player.PlaylistSavingEvent += (sender, e) => { _skin.Render(e.Message); };
            _player.WriteLyricsEvent += (sender, e) => { _skin.Render(e.Message); };
            _player.Load();
            ShowSongsList();
            int p = 0;
            while (p != 1)
            {
                if (!_player.IsLock)
                {
                    switch (ReadKey(true).Key)
                    {
                        case ConsoleKey.A:
                            {
                                _player.Load();
                                ShowSongsList();
                                break;
                            }

                        case ConsoleKey.D0:
                            {
                                foreach (var song in _player.Songs)
                                {
                                    _player.PlaySongAsync(song);
                                    ShowSongsList();
                                }
                                break;
                            }
                        case ConsoleKey.NumPad0:
                            {
                                foreach (var song in _player.Songs)
                                {
                                    
                                    _player.PlaySongAsync(song);

                                }
                                break;
                            }

                        case ConsoleKey.C:
                            {
                                _player.Clear();
                                ShowSongsList();
                                break;
                            }

                        case ConsoleKey.Escape:
                            {
                                p = 1;
                                break;
                            }

                        case ConsoleKey.P:
                            {
                                try
                                {
                                    _skin.Render("Введите номер песни для воспроизведения");
                                    int number = int.Parse(ReadLine());
                                   // _player.Songs[number - 1].Playing = true;
                                    
                                    _player.PlaySongAsync(_player.Songs[number - 1]);
                                    ShowSongsList();
                                }
                                catch (Exception e)
                                {
                                    _skin.Render(e.Message);
                                }

                                break;
                            }

                        case ConsoleKey.U:
                            {
                                _player.VolumeUP();
                                ShowSongsList();
                                break;
                            }

                        case ConsoleKey.D:
                            {
                                _player.VolumeDown();
                                ShowSongsList();
                                break;
                            }

                        case ConsoleKey.L:
                            {
                                _player.LockOn();
                                ShowSongsList();
                                break;
                            }

                        case ConsoleKey.X:
                        {
                            _player.LoadPlayList();
                            break;
                        }

                        case ConsoleKey.V:
                        {
                            _player.SaveAsPlaylist();
                            break;
                        }

                        case ConsoleKey.W:
                        {
                            try
                            {
                                _skin.Render("Введите номер песни");
                                int number = int.Parse(ReadLine());
                                _player.WriteLyrics(_player.Songs[number-1]);
                            }
                            catch (Exception e)
                            {
                                _skin.Render(e.Message);
                            }
                            break;
                        }
                    }

                }

                switch (ReadKey(true).Key)
                {
                    case ConsoleKey.N:
                        {
                            _player.LockOff();
                            ShowSongsList();
                            break;
                        }
                }
            }
            _player.Dispose();
        }

        private static void _player_LockOnEvent(Player sender, PlayerEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static string ChooseColor()
        {
            _skin.Render("Выберете цвет из списка");
            for (int i = 0; i < 15; i++)
            {
                _skin.Render($"{i + 1}. {Enum.Parse(typeof(ConsoleColor), i.ToString())}");
            }
            WriteLine();
            int number = int.Parse(ReadLine());
            number--;
            return number.ToString();
        }

        private static void ShowSongsList()
        {
            int i = 0;
            _skin.Clear();
            _skin.Render("Для воспроизведения песни по номеру нажмите P");
            _skin.Render("Для воспроизведения всех песен введите 0");
            _skin.Render("Для загрузки песен нажмите A");
            _skin.Render("Для очистки списка песен нажмите C");
            _skin.Render("Для увеличения громкости нажмите U");
            _skin.Render("Для уменьшения громкости нажмите D");
            _skin.Render("Чтобы заблокировать плеер нажмите L");
            _skin.Render("Чтобы разблокировать плеер нажмите N");
            _skin.Render("Для закрузки плейлиста нажмите X");
            _skin.Render("Для сохранения плейлиста нажмите V");
            _skin.Render("Чтобы вывести текст песни нажмите W");
            _skin.Render("Для выхода нажмите ESC");
            WriteLine();
            _skin.Render("Текущий плейлист:");
            foreach (var song in _player.Songs)
            {
                if (song.Artist.Name == null)
                {
                    song.Artist.Name = "Unknwon artist";
                }
                if (song.Title == null)
                {
                    song.Title = "No title";
                }
                i++;
                if (song.Playing)
                {

                    _skin.Render($"{i}. {song.Artist.Name} - {song.Title}" + "" + "- Playing now");
                }
                else if (song.LikeStatus.HasValue)
                {
                    if (song.LikeStatus == true)
                    {
                        _skin.Render($"{i}. {song.Artist.Name} - {song.Title}" + "" + "- Liked");
                    }
                    else
                    {
                        _skin.Render($"{i}. {song.Artist.Name} - {song.Title}" + "" + "- Disliked");
                    }
                }
                else
                {
                    _skin.Render($"{i}. {song.Artist.Name} - {song.Title}" + "" + "- Undefined");
                }
            }

            if (_player.IsLock)
            {
                _skin.Render($"Player status");
                _skin.Render("Заблокирован - да");
                _skin.Render($"Уровень громкости - {_player.Volume.ToString()}");
            }
            else
            {
                _skin.Render($"Player status");
                _skin.Render("Заблокирован - нет");
                _skin.Render($"Уровень громкости - {_player.Volume.ToString()}");
            }
        }
    }
}
