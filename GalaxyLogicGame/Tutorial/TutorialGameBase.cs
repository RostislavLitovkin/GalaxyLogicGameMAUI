using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Planet_objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Tutorial
{
    public abstract class TutorialGameBase : CasualGame
    {
        private int clickableIndex;

        public TutorialGameBase()
        {
            // nothing here
        }
        
        public override async Task Setup()
        {
            await SetupTutorial();
            Clicked = true;

        }
        public override void SetupBase()
        {
            Clicked = false;
            base.SetupBase();
            
            BG.LowerUILayout.IsVisible = false;
            BG.PowerupsAllowed = false;
            if (Device.RuntimePlatform == Device.Tizen) { BG.BackgroundImage.Source = "starsSky.jpg"; AbsoluteLayout.SetLayoutBounds(BG.BackgroundImage, new Rect(0.5, 0.5, 360, 360)); }
            else
            {
                AbsoluteLayout.SetLayoutBounds(BG.BackgroundImage, new Rect(0.5, 0.5, 480, 480));
                BG.BackgroundImage.Source = "starssky.png";
                BG.TransitionLayout.BackgroundColor = Color.FromArgb("f00");
            }
            BG.TutorialLayout.IsVisible = true;
        }

        public abstract Task SetupTutorial();

        public override async Task AreaClicked(int index)
        {
            if (Clicked)
            {
                Clicked = false;

                await AddingPlanet(index);

                AddTutorialClickableArea(index);

                Clicked = true;
            }
        }
        public override async Task AddingPlanet(int index)
        {
            for (int i = 0; i < ClickableAreas.Count; i++)
            {
                if (ClickableAreas.Count > i) try { ((ClickableAreaWithParticles)ClickableAreas[i]).FadeTo(0, 250); } catch { }
            }

            await base.AddingPlanet(index);

            ClickableAreaLayout.Children.Clear();
            ClickableAreas.Clear();
        }

        public async Task TutorialText(string text, int index)
        {

            Label label = new Label
            {
                Text = text,
                TextColor = Color.FromArgb("fff"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 24,
            };
            AbsoluteLayout tempLayout = new AbsoluteLayout
            {
                BackgroundColor = Color.FromHex("88000000"),
                Opacity = 0,
            };
            tempLayout.IsVisible = true;
            tempLayout.Children.Add(label);
            AbsoluteLayout.SetLayoutBounds(label, new Rect(0.5, 0.5, 320, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.PositionProportional);
            BG.TutorialLayout.Children.Add(tempLayout);
            AbsoluteLayout.SetLayoutBounds(tempLayout, new Rect(0.5, 0.5, 2, 2));
            AbsoluteLayout.SetLayoutFlags(tempLayout, AbsoluteLayoutFlags.All);

            tempLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () => {
                    if (Clicked)
                    {
                        Clicked = false;
                        await tempLayout.FadeTo(0, 500);
                        tempLayout.IsVisible = false;
                        AddTutorialClickableArea(index);

                        BG.TutorialLayout.Children.Remove(tempLayout);
                        Clicked = true;
                    }
                })
            });

            await Task.Delay(500);
            await tempLayout.FadeTo(1, 250);
        }


        public async Task TutorialText(string text, int index, Command onClickCommand)
        {

            Label label = new Label
            {
                Text = text,
                TextColor = Color.FromArgb("fff"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 24,
            };
            AbsoluteLayout tempLayout = new AbsoluteLayout
            {
                BackgroundColor = Color.FromHex("88000000"),
                Opacity = 0,
            };
            tempLayout.Children.Add(label);
            AbsoluteLayout.SetLayoutBounds(label, new Rect(0.5, 0.5, 320, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.PositionProportional);
            BG.TutorialLayout.Children.Add(tempLayout);
            AbsoluteLayout.SetLayoutBounds(tempLayout, new Rect(0.5, 0.5, 2, 2));
            AbsoluteLayout.SetLayoutFlags(tempLayout, AbsoluteLayoutFlags.All);

            tempLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = onClickCommand
            });

            await Task.Delay(500);
            await tempLayout.FadeTo(1, 250);
        }


        public async Task SecondTutorialText(string text, TutorialGameBase nextTutorial)
        {
            BG.TutorialLayout.IsVisible = true;
            Label label = new Label
            {
                Text = text,
                TextColor = Color.FromArgb("fff"),

                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 24,

            };
            AbsoluteLayout tempLayout = new AbsoluteLayout
            {
                BackgroundColor = Color.FromHex("88000000"),
                Opacity = 0,
            };
            tempLayout.Children.Add(label);
            AbsoluteLayout.SetLayoutBounds(label, new Rect(0.5, 0.5, 320, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.PositionProportional);
            BG.TutorialLayout.Children.Add(tempLayout);
            AbsoluteLayout.SetLayoutBounds(tempLayout, new Rect(0.5, 0.5, 2, 2));
            AbsoluteLayout.SetLayoutFlags(tempLayout, AbsoluteLayoutFlags.All);



            tempLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    if (Clicked)
                    {
                        Clicked = false;
                        await tempLayout.FadeTo(0, 500);
                        tempLayout.IsVisible = false;
                        BG.TutorialLayout.Children.Remove(tempLayout);

                        await BG.NavigateToOtherTutorial(nextTutorial);
                    }
                })
            });

            await Task.Delay(500);
            await tempLayout.FadeTo(1, 250);
        }

        public void AddTutorialClickableArea(int index)
        {
            clickableIndex = index;
            BG.TutorialLayout.IsVisible = false;

            ClickableAreaLayout.Children.Clear();
            ClickableAreas.Clear();

            double degre = CirclePosition.CalculateDegre(Atoms.Count);
            ArrayList positions = CirclePosition.GetCirclePositionsOffset(Atoms.Count, Offset + degre / 2);

            ClickableAreaWithParticles area = new ClickableAreaWithParticles
            {
                Index = index,
                Opacity = 1,
            };
            ClickableAreas.Add(area);
            area.Play();
            area.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await AreaClicked(area.Index)) });
            //try { area.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { try { await areaClicked(area.Index); } catch (Exception ex) { } }) }); } catch { }
            //AbsoluteLayout.SetLayoutBounds(b, new Rect(150, 150, 60, 60)); //you will have to change values here, if you change the size
            Position p = (Position)positions[index];

            if (Atoms.Count < 5)
            {
                int size = 500 / Atoms.Count;
                AbsoluteLayout.SetLayoutBounds(area, new Rect(p.X + 180 - size / 2, p.Y + 180 - size / 2, size, size));
            }
            else AbsoluteLayout.SetLayoutBounds(area, new Rect(p.X + 145, p.Y + 145, 70, 70));
            //AbsoluteLayout.SetLayoutFlags(b, AbsoluteLayoutFlags.PositionProportional);

            ClickableAreaLayout.Children.Add(area);

            BoxViewWithIndex area2 = new BoxViewWithIndex
            {
                BackgroundColor = Color.FromArgb("0000"),
            };
            ClickableAreas.Add(area2);
            var gesture = new PanGestureRecognizer { };
            gesture.PanUpdated += PanGestureRecognizer_PanUpdated;
            area2.GestureRecognizers.Add(gesture);
            AbsoluteLayout.SetLayoutBounds(area2, new Rect(145, 145, 70, 70));
            ClickableAreaLayout.Children.Add(area2);
        }

        private Queue<(float x, float y)> _positions = new Queue<(float, float)>();

        async void PanGestureRecognizer_PanUpdated(System.Object sender, Microsoft.Maui.Controls.PanUpdatedEventArgs e)
        {
            if (NewPlanet != null)
            {
                if (e.StatusType == GestureStatus.Running)
                {
                    DisplayInfo displayInfo = DeviceDisplay.MainDisplayInfo;

                    _positions.Enqueue(((float)e.TotalX, (float)e.TotalY));
                    if (_positions.Count > 10)
                        _positions.Dequeue();

                    float xAverage = _positions.Average(item => item.x);
                    float yAverage = _positions.Average(item => item.y);

                    NewPlanet.TranslationX = xAverage;
                    NewPlanet.TranslationY = yAverage;

                    int distance = (int)Math.Sqrt(Math.Pow(xAverage, 2) + Math.Pow(yAverage, 2));

                    if (distance > 110 && distance < 200)
                    {
                        double angle = ((Math.Atan2(xAverage, yAverage) + 2 * Math.PI) * 180) / Math.PI;

                        int pos = (int)((angle - (Offset) + 360) % 360 / (360 / Atoms.Count));

                        Console.WriteLine(angle.ToString());
                        Console.WriteLine((pos % 360).ToString());

                        if (pos == clickableIndex)
                        {
                            await PutBetweenPlanetPreview(pos);
                        }
                        else
                        {
                            await MoveAtoms();
                        }
                    }
                    else
                    {
                        await MoveAtoms();
                    }
                }
                else if (e.StatusType == GestureStatus.Completed)
                {
                    float xAverage = _positions.Average(item => item.x);
                    float yAverage = _positions.Average(item => item.y);

                    int distance = (int)Math.Sqrt(Math.Pow(xAverage, 2) + Math.Pow(yAverage, 2));
                    Console.WriteLine(distance.ToString());
                    if (distance > 110 && distance < 200)
                    {
                        double angle = ((Math.Atan2(xAverage, yAverage) + 2 * Math.PI) * 180) / Math.PI;

                        int pos = (int)((angle - (Offset) + 360) % 360 / (360 / Atoms.Count));

                        _positions = new Queue<(float, float)>();
                        if (pos == clickableIndex)
                        {
                            await AreaClicked(pos);
                        }
                        else
                        {
                            _positions = new Queue<(float, float)>();
                            await NewPlanet.TranslateTo(0, 0, 250);
                        }
                    }
                    else
                    {
                        _positions = new Queue<(float, float)>();
                        await NewPlanet.TranslateTo(0, 0, 250);
                    }
                }
            }
        }

        private async Task PutBetweenPlanetPreview(int index)
        {
            if (index < 0)
            {
                Console.WriteLine(index);
                return;
            }
            // Calculating offset
            double degrePreview = CirclePosition.CalculateDegre(Atoms.Count + 1);
            double degreClickedPreview = Offset + Degre / 2 + index * Degre;
            int tempIndexPreview = (Atoms.Count + 1 - index - 1) % (Atoms.Count + 1);
            double offsetPreview = (degrePreview * tempIndexPreview + degreClickedPreview) % 360;


            ArrayList positions = CirclePosition.GetCirclePositionsOffset(Atoms.Count + 1, offsetPreview);

            for (int i = 0; i < positions.Count; i++)
            {
                if (i < index + 1)
                {
                    Position p = (Position)positions[i];
                    ((PlanetBase)Atoms[i % Atoms.Count]).TranslateTo(p.X, p.Y, (uint)Delay);
                }
                else if (i == index + 1)
                {

                }
                else
                {
                    Position p = (Position)positions[i];
                    ((PlanetBase)Atoms[(i - 1) % Atoms.Count]).TranslateTo(p.X, p.Y, (uint)Delay);
                }
            }
        }

        public override void LoopTick()
        {
            if (MiddleParticle != null && MiddleParticle.IsPlaying)
            {
                MiddleParticle.Move();
            }
            
            if (ClickableAreas.Count > 0)
            {
                for (int i = 0; i < ClickableAreas.Count; i++)
                {
                    if (ClickableAreas.Count > i) try { ((ClickableAreaWithParticles)ClickableAreas[i]).Move(); } catch { }
                }
            }
       
        }
        public void AddNewPlanet(int value)
        {
            NewPlanet = new Planet { Type = 0, Text = value + "" };
            AtomsLayout.Children.Add(NewPlanet);
        }
        public void AddNewPlanet(PlanetBase planet)
        {
            NewPlanet = planet;
            AtomsLayout.Children.Add(NewPlanet);
        }

        
    }
}
