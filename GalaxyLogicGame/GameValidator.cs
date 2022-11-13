using System.Collections;
using GalaxyLogicGame.Planet_objects;
using Microsoft.Maui.Controls;
using GalaxyLogicGame.Events;

namespace GalaxyLogicGame
{
    public class GameValidator
    {
        public GameValidator()
        {

        }

        public static (PseudoRandomGenerator PseudoRNG, ArrayList Planets, int Score) ValidateSaved()
        {
            Console.WriteLine("Check running");

            string[] gameString = Preferences.Get("save", "").Split(' ');
            int score = 0;
            ArrayList stars = new ArrayList(1);
            ArrayList planets = new ArrayList(15);
            ArrayList shrinkingGiants = new ArrayList(3);
            int limit = 14;
            int eventCounter = 0;
            int plusWaiting = 3;
            bool starCountdown = false;
            bool generateNewPlanet = true;
            PlanetBase newPlanet = new Planet();


            
            ThreeInRowEvent tir;

            (int Duration, string Name, int Value) eventObject = (0, null, -1); // duration, name, value

            int highest = 7;
            int lowest = 1;
            int turn = 0;
            // initialize RNG
            PseudoRandomGenerator pseudoRNG = new PseudoRandomGenerator(Int32.Parse(gameString[0])); ;

            // game mode
            // ignored for now


            eventCounter = pseudoRNG.Next(-10, -3); // makes you fucking lucky

            // Generate the board
            for (int i = 0; i < 7; i++)
            {
                planets.Add(new Planet { Type = 0, Text = pseudoRNG.Next(1, 7).ToString() });
            }

            // individual turns
            for (int s = 2; s < gameString.Length; s++)
            {
                int index = int.Parse(gameString[s]);

                if (false)
                {
                    // Telescope power-up things
                }

                // Generate new planet
                if (generateNewPlanet)
                {
                    if (pseudoRNG.Next(5) == 0 || plusWaiting >= 6) //    <-- collision / merge
                    {
                        newPlanet = new Planet
                        {
                            Type = 1,
                            Text = "+",
                            TextColor = Color.FromArgb("fff"),
                            BGColor = Color.FromArgb("f00"),
                        };
                        if (plusWaiting >= 6) plusWaiting = 0;
                        else plusWaiting = 2;
                        starCountdown = true;
                    }

                    else if ((pseudoRNG.Next(20) == 0 && planets.Count > 3) || (pseudoRNG.Next(10) == 0 && planets.Count > 10)) // <-- blackhole
                    {
                        starCountdown = false;

                        turn++;

                        if (((PlanetBase)planets[index]).Type == 2)
                        {
                            stars.Remove((Supernova)planets[index]);

                            newPlanet = GenerateShrinkingGiant(ref shrinkingGiants, ref highest);

                        }
                        else newPlanet = (PlanetBase)planets[index];

                        planets.RemoveAt(index);

                        MergePlanets(ref planets, ref highest, ref score, ref shrinkingGiants, ref limit, ref plusWaiting);

                        if (shrinkingGiants.Count > 0)
                        {
                            for (int i = 0; i < shrinkingGiants.Count; i++)
                            {
                                if (!((Planet)shrinkingGiants[i]).Text.Equals("1")) ((Planet)shrinkingGiants[i]).Text = (int.Parse(((Planet)shrinkingGiants[i]).Text) - 1).ToString();
                            }
                        }

                        s++;
                        index = int.Parse(gameString[s]);
                    }

                    else if (pseudoRNG.Next(20) == 0 && stars.Count < 1) // <-- star
                    {

                        PlanetBase temporaryPlanet = new Supernova
                        {
                            Text = pseudoRNG.Next(3, 9).ToString()
                        };
                        starCountdown = false;
                        newPlanet = temporaryPlanet;
                        stars.Add(temporaryPlanet);
                    }

                    //                          <-- some more
                    else // <-- normal planet
                    {
                        plusWaiting++;
                        starCountdown = true;
                        int v = pseudoRNG.Next(lowest, highest);
                        newPlanet = new Planet
                        {
                            Type = 0,
                            Text = v.ToString(),
                        };
                    }
                }

                // Area clicked
                turn++;

                if (index + 1 < planets.Count)
                {
                    planets.Insert(index + 1, newPlanet);
                }
                else
                {
                    planets.Add(newPlanet);
                }

                // if (binary != null) await binary.CheckFormation();

                MergePlanets(ref planets, ref highest, ref score, ref shrinkingGiants, ref limit, ref plusWaiting);

                // Star behaviour
                if (stars.Count > 0 && starCountdown)
                {
                    for (int i = 0; i < stars.Count; i++)
                    {
                        ((PlanetBase)stars[i]).Text = Functions.GetAtomValue((PlanetBase)stars[i]) - 1 + "";
                        if (Functions.GetAtomValue((PlanetBase)stars[i]) == 0)
                        {

                            // Star explosion
                            for (int j = 0; j < planets.Count; j++)
                            {
                                if (((PlanetBase)planets[j]).Type == 2 && Functions.GetAtomValue((PlanetBase)planets[j]) == 0)
                                {
                                    Supernova explodingStar = (Supernova)planets[j];
                                    int low;
                                    int high;
                                    int tempI = 0;

                                    if (j != 0) low = j - 1;
                                    else low = planets.Count - 1;
                                    if (j != planets.Count - 1) high = j + 1;
                                    else high = 0;

                                    if (low < j) tempI = low;
                                    else tempI = j;
                                    if (low > high && low < j) tempI--;


                                    stars.Remove((PlanetBase)planets[j]);

                                    planets.RemoveAt(low);
                                    if (planets.Count > low) planets.RemoveAt(low);
                                    else planets.RemoveAt(0);

                                    if (planets.Count > 0)
                                    {
                                        if (planets.Count > low) planets.RemoveAt(low);
                                        else planets.RemoveAt(0);
                                    }

                                    if (planets.Count != 0)
                                    {
                                        if (planets.Count > limit - 3)
                                        {
                                            plusWaiting++;
                                        }
                                    }

                                    break;
                                }
                            }


                            UpdateShrinkingGiantsArray(ref planets, ref shrinkingGiants);

                            if (planets.Count <= 1)
                            {
                                Planet temporaryButton = new Planet();
                                int v = pseudoRNG.Next(lowest, highest + 1);

                                temporaryButton.Type = 0;
                                temporaryButton.Text = v + "";

                                planets.Add(temporaryButton);
                            }
                            else
                            {
                                MergePlanets(ref planets, ref highest, ref score, ref shrinkingGiants, ref limit, ref plusWaiting);
                            }
                            break;
                        }
                    }
                    starCountdown = false;
                }

                if (planets.Count > limit - 3)
                {
                    plusWaiting++;
                }

                generateNewPlanet = true;

                
                // Event object move
                if (eventObject.Duration != 0)
                {
                    eventObject.Duration--;

                    if (eventObject.Duration == 0)
                    {
                        if (eventObject.Name == "ThreeInRow")
                        {
                            eventObject = (0, null, -1);
                        }
                        else if (eventObject.Name == "Dreams")
                        {
                            for (int i = 0; i < planets.Count; i++)
                            {
                                if (((PlanetBase)planets[i]).Type == 0) ((Planet)planets[i]).DreamNumber = 0;
                            }

                            eventObject = (0, null, -1);
                        }

                    }
                }

                

                // determine event
                if (true) // (!TelescopeActivated)
                {
                    eventCounter++;

                    Console.WriteLine(eventCounter);
                    if (eventCounter < 3)
                    {
                        if (eventObject.Name == "ThreeInRow")
                        {
                            generateNewPlanet = false;

                            newPlanet = new Planet
                            {
                                Type = 0,
                                Text = eventObject.Value.ToString()
                            };
                            starCountdown = true;
                            plusWaiting++;
                        }
                    }
                    else
                    {
                        eventCounter = -1; // default value - can differ depending on the event

                        //if (blindness != null) await blindness.Disappear(this);
                        //else if (tir != null) await tir.Disappear(this); // this will get changed ...

                        //if (eventObject != null) await eventObject.Move(); // hopefully this works - BTW this replaces the 2 lines above

                        ArrayList events = new ArrayList(12);
                        for (int i = 0; i < 12; i++)
                        {
                            events.Add(i);
                        }

                        // check event prerequisites
                        bool eventPrerequisites = false;
                        while (!eventPrerequisites)
                        {
                            if (events.Count != 0)
                            {
                                int eventIndex = pseudoRNG.Next(events.Count);
                                int eventNum = (int)events[eventIndex];
                                events.RemoveAt(eventIndex);

                                if (eventNum == 0) // shooting star
                                {
                                    eventPrerequisites = true;
                                    Console.WriteLine("Shooting star");

                                    PlanetBase[] planetsChoice = new PlanetBase[3];
                                    generateNewPlanet = false;

                                    // 3 random planets
                                    for (int i = 0; i < 3; i++)
                                    {
                                        PlanetBase planet;

                                        if (i == 0)
                                        {
                                            int type = pseudoRNG.Next(1, 3);


                                            if (type == 2 && stars.Count > 0) type--;
                                            if (type == 1)
                                            {
                                                planet = new Planet
                                                {
                                                    Type = 1,
                                                    Text = "+",
                                                    BGColor = Color.FromArgb("f00"),
                                                    TextColor = Color.FromArgb("fff"),
                                                };
                                            }
                                            else if (type == 2)
                                            {
                                                planet = new Supernova
                                                {
                                                    Text = pseudoRNG.Next(3, 9).ToString()
                                                };
                                            }
                                            else
                                            {
                                                planet = new Planet();
                                                // Error
                                            }
                                        }
                                        else
                                        {
                                            int v = pseudoRNG.Next(lowest, highest);
                                            planet = new Planet
                                            {
                                                Type = 0,
                                                Text = v.ToString(),
                                            };

                                        }

                                        planetsChoice[i] = planet;
                                    }

                                    // choice click
                                    s++;
                                    index = int.Parse(gameString[s]);
                                    newPlanet = planetsChoice[index];
                                    plusWaiting++;
                                    starCountdown = true;
                                    //else if (planetsChoice[index].Type == 1) ;
                                    if (planetsChoice[index].Type == 2) {
                                        starCountdown = false;
                                        stars.Add(planetsChoice[index]);
                                    }

                                    //await game.UpdateShrinkingGiants();
                                }
                                else if (eventNum == 1) // blindness
                                {
                                    eventPrerequisites = true;
                                    eventCounter = -5;
                                    Console.WriteLine("Blindness");

                                }
                                else if (eventNum == 2) // three in row
                                {
                                    eventPrerequisites = true;
                                    Console.WriteLine("Three in row");

                                    eventObject = (3, "ThreeInRow", pseudoRNG.Next(lowest, highest));
                                    eventCounter = -2;

                                    generateNewPlanet = false;
                                    newPlanet = new Planet
                                    {
                                        Type = 0,
                                        Text = eventObject.Value.ToString()
                                    };
                                    starCountdown = true;
                                    plusWaiting++;
                                }
                                else if (eventNum == 3) // polymorph
                                {
                                    if (stars.Count == 0 /*&& debris.Count == 0*/ && planets.Count > 2)
                                    {
                                        Console.WriteLine("Polymorph");
                                        eventPrerequisites = true;

                                        int chosenIndex = pseudoRNG.Next(planets.Count);

                                        // clicked
                                        s++;
                                        index = int.Parse(gameString[s]);
                                        if (((PlanetBase)planets[chosenIndex]).IsTypeThree)
                                        {
                                            ((PlanetBase)planets[chosenIndex]).Type = 3;
                                        } 
                                        else ((PlanetBase)planets[chosenIndex]).Type = ((PlanetBase)planets[index]).Type;

                                        ((Planet)planets[chosenIndex]).Text = ((Planet)planets[index]).Text;
                                        ((Planet)planets[chosenIndex]).TextColor = ((Planet)planets[index]).TextColor;
                                        ((Planet)planets[chosenIndex]).BGColor = ((Planet)planets[index]).BGColor;

                                        UpdateShrinkingGiantsArray(ref planets, ref shrinkingGiants);

                                        MergePlanets(ref planets, ref highest, ref score, ref shrinkingGiants, ref limit, ref plusWaiting);
                                    }
                                }
                                else if (eventNum == 4) // binary
                                {
                                    // ignore for now
                                }
                                else if (eventNum == 5) // astronaut
                                {
                                    Console.WriteLine("astronaut");

                                    eventPrerequisites = true;
                                }
                                else if (eventNum == 6) // dreams
                                {
                                    int counter = 0;
                                    if (planets.Count >= 4)
                                    {
                                        for (int i = 0; i < planets.Count; i++)
                                        {
                                            if (((PlanetBase)planets[i]).Type == 0) counter++;
                                        }
                                    }
                                    if (counter >= 3)
                                    {
                                        Console.WriteLine("Dreams");
                                        eventPrerequisites = true;
                                        eventObject = (5, "Dreams", -1);
                                        eventCounter = -2; // change this

                                        ArrayList availablePlanets = new ArrayList();
                                        for (int i = 0; i < planets.Count; i++)
                                        {
                                            if (((PlanetBase)planets[i]).Type == 0) availablePlanets.Add(planets[i]);
                                        }

                                        // generate dream numbers
                                        for (int dreamNum = 1; dreamNum <= 3; dreamNum++)
                                        {
                                            int dreamIndex = pseudoRNG.Next(availablePlanets.Count);
                                            ((Planet)availablePlanets[dreamIndex]).DreamNumber = dreamNum;
                                            availablePlanets.RemoveAt(dreamIndex);
                                        }

                                        MergePlanets(ref planets, ref highest, ref score, ref shrinkingGiants, ref limit, ref plusWaiting);
                                    }
                                }
                                else if (eventNum == 7) // atomic bomb
                                {
                                    // ignore for now
                                }
                                else if (eventNum == 8) // christmas
                                {
                                    // ignore for now
                                }
                                else if (eventNum == 9) // WorldUpsideDown 
                                {
                                    // ignore for now
                                }
                                else if (eventNum == 10) // Blueberries
                                {
                                    // ignore for now
                                }
                                else if (eventNum == 11) // WorldUpsideDown (again on purpose XD)
                                {
                                    // ignore for now
                                }
                                else // Nothing event
                                {
                                    // This is just nothing, but it can also mean a bug somewhere

                                    Console.WriteLine("Nothing event");
                                    eventPrerequisites = true;
                                }
                            }
                            else
                            {
                                // Error

                                eventPrerequisites = true;
                            }
                        }
                    }
                }

                // Update shrinking giants
                if (shrinkingGiants.Count > 0)
                {
                    for (int i = 0; i < shrinkingGiants.Count; i++)
                    {
                        if (!((Planet)shrinkingGiants[i]).Text.Equals("1")) ((Planet)shrinkingGiants[i]).Text = (int.Parse(((Planet)shrinkingGiants[i]).Text) - 1).ToString();
                    }

                    MergePlanets(ref planets, ref highest, ref score, ref shrinkingGiants, ref limit, ref plusWaiting);
                }

                //await BG.LostScreenAnimation(); // Check later
            }

            return (pseudoRNG, planets, score);
        }

        /**
         * Return gained score
         */
        private static void MergePlanets(ref ArrayList planets, ref int highest, ref int score, ref ArrayList shrinkingGiants,
            ref int limit, ref int plusWaiting)
        {
            bool repeatTwice = true;

            int planetValue = 0;
            int planetBonus = 0;
            PlanetBase[] selectedPlanet = new PlanetBase[1];
            while (repeatTwice)
            {
                repeatTwice = false;

                GoHere:

                for (int i = 0; i < planets.Count; i++)
                {
                    if (((PlanetBase)planets[i]).Type == 1)
                    {
                        int low, high;


                        if (i == 0) low = planets.Count - 1;
                        else low = i - 1;
                        if (i == planets.Count - 1) high = 0;
                        else high = i + 1;

                        for (int dreamLow = 0; dreamLow < ((PlanetBase)planets[low]).DreamNumber + 1; dreamLow++)
                        {
                            for (int dreamHigh = 0; dreamHigh < ((PlanetBase)planets[high]).DreamNumber + 1; dreamHigh++)
                            {
                                if (((PlanetBase)planets[low]).Type == 0 && ((PlanetBase)planets[high]).Type == 0 && (GetPlanetValue((PlanetBase)planets[low]) + dreamLow) == (GetPlanetValue((PlanetBase)planets[high]) + dreamHigh) && low != high)
                                {
                                    score += (GetPlanetValue((PlanetBase)planets[low]) + dreamLow) * 2 + planetBonus * planetBonus * 5;

                                    repeatTwice = true;
                                    double x = ((PlanetBase)planets[i]).TranslationX;
                                    double y = ((PlanetBase)planets[i]).TranslationY;
                                    //debuggingLabel.Text = x + ""; // for debugging

                                    if (planetValue < GetPlanetValue((PlanetBase)planets[high]) + dreamHigh)
                                    {
                                        planetValue = GetPlanetValue((PlanetBase)planets[high]) + dreamHigh;
                                    }
                                    planetBonus++;

                                    if (planetValue + planetBonus > highest) highest = planetValue + planetBonus;

                                    selectedPlanet[0] = (PlanetBase)planets[i];

                                    planets.RemoveAt(low);

                                    if (low < high)
                                    {
                                        if (!(high - 1 < 0))
                                        {
                                            planets.RemoveAt(high - 1);
                                        }
                                        else
                                        {
                                            planets.RemoveAt(planets.Count - 1);
                                        }
                                    }
                                    else //if (atoms.Count > 1) // maybe a useless check
                                    {
                                        planets.RemoveAt(high);
                                    }

                                    goto GoHere;
                                }
                            }
                        }
                    }
                }
            }
            if (planetValue != 0)
            {
                selectedPlanet[0].Type = 0;
                selectedPlanet[0].Text = (planetValue + planetBonus).ToString();

                // if (BinaryActivated) selectedAtom[0].Binary.IsVisible = true;

                UpdateShrinkingGiantsArray(ref planets, ref shrinkingGiants);

                MergePlanets(ref planets, ref highest, ref score, ref shrinkingGiants, ref limit, ref plusWaiting);
            }

            // maybe a gameover check
            if (planets.Count > limit - 3)
            {
                plusWaiting++;
            }
        }

        private static PlanetBase GenerateShrinkingGiant(ref ArrayList shrinkingGiants, ref int highest)
        {
            highest += 3;
            Planet temporaryButton = new Planet
            {
                Type = 3,
                Text = (highest + 1).ToString(),
                BGColor = Color.FromArgb("000"),
                TextColor = Color.FromArgb("be66ed"),
            };
            shrinkingGiants.Add(temporaryButton);

            return temporaryButton;
        }

        private static void UpdateShrinkingGiantsArray(ref ArrayList planets, ref ArrayList shrinkingGiants)
        {
            shrinkingGiants.Clear();

            for (int i = 0; i < planets.Count; i++)
            {
                if (((PlanetBase)planets[i]).IsTypeThree) shrinkingGiants.Add(planets[i]);
            }
        }

        private static int GetPlanetValue(PlanetBase p)
        {
            return int.Parse(p.Text);
        }
    }
}

