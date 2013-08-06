﻿namespace Grove.UserInterface.StartScreen
{
  using System;
  using System.Diagnostics;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using System.Windows;
  using Gameplay;
  using Infrastructure;
  using Persistance;
  using SelectDeck;

  public class ViewModel : ViewModelBase
  {
    private const string YourName = "You";

    public string DatabaseInfo
    {
      get
      {
        return string.Format("Release {0} ({1} cards)",
          FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion,
          CardsDictionary.Count);
      }
    }

    public void Exit()
    {
      Application.Current.Shutdown();
    }

    public void NewTournament()
    {
      var newTournament = ViewModels.NewTournament.Create(this);
      Shell.ChangeScreen(newTournament);
    }

    public void LoadSavedGame()
    {
      var loadSavedGame = ViewModels.LoadSavedGame.Create(this);
      Shell.ChangeScreen(loadSavedGame);
    }

    public void Play()
    {
      SelectDeck.ViewModel selectDeck1 = null;
      SelectDeck.ViewModel selectDeck2 = null;

      var configuration1 = new Configuration
        {
          ScreenTitle = "Select your deck",
          ForwardText = "Next",
          PreviousScreen = this,
          Forward = (deck1) =>
            {
              if (selectDeck2 == null)
              {
                selectDeck2 = ViewModels.SelectDeck.Create(new Configuration
                  {
                    ScreenTitle = "Select your opponent deck",
                    ForwardText = "Start the game",
                    PreviousScreen = selectDeck1,
                    Forward = (deck2) =>
                      {
                        try
                        {
                          MatchRunner.Start(
                            player1: new PlayerParameters
                              {
                                Name = YourName,
                                AvatarId = RandomEx.Next(),
                                Deck = deck1
                              },
                            player2: new PlayerParameters
                              {
                                Name = MediaLibrary.NameGenerator.GenerateName(),
                                AvatarId = RandomEx.Next(),
                                Deck = deck2
                              },
                            isTournament: false
                            );
                        }
                        catch (Exception ex)
                        {
                          HandleException(ex);
                        }

                        Shell.ChangeScreen(this);
                      }
                  });
              }

              Shell.ChangeScreen(selectDeck2);
            }
        };

      selectDeck1 = ViewModels.SelectDeck.Create(configuration1);
      Shell.ChangeScreen(selectDeck1);
    }

    public void PlayRandom()
    {
      Deck firstDeck;
      Deck secondDeck;

      ChooseRandomDecks(out firstDeck, out secondDeck);

      try
      {
        MatchRunner.Start(
          player1: new PlayerParameters
            {
              Name = YourName,
              AvatarId = RandomEx.Next(),
              Deck = firstDeck
            },
          player2: new PlayerParameters
            {
              Name = MediaLibrary.NameGenerator.GenerateName(),
              AvatarId = RandomEx.Next(),
              Deck = secondDeck
            },
          isTournament: false
          );
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }


      Shell.ChangeScreen(this);
    }

    public void DeckEditor()
    {
      var deckEditorScreen = ViewModels.DeckEditor.Create(this);
      Shell.ChangeScreen(deckEditorScreen);
    }

    private void ChooseRandomDecks(out Deck firstDeck, out Deck secondDeck)
    {
      var deckFiles = Directory
        .EnumerateFiles(MediaLibrary.DecksFolder, "*.dec");

      var decks = deckFiles
        .Select(DeckFile.Read)
        .ToList();

      var first = decks[RandomEx.Next(0, decks.Count)];

      var decksWithSameRating = decks
        .Where(x => x.Rating == first.Rating)
        .ToList();

      var second = decksWithSameRating[RandomEx.Next(0, decksWithSameRating.Count)];

      firstDeck = first;
      secondDeck = second;
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}