﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Text_Parser
{
    class Program
    {
        //messages/day, specific emojis/day, words/day
        static Dictionary<string, string> _emojiLookup = new Dictionary<string, string>();
        static ConcurrentQueue<LineDataModel> _messageParseQueue = new ConcurrentQueue<LineDataModel>();
        static bool _finishedReading = false;

        static void Main(string[] args)
        {
            var inputFilePath = @"C:\Users\Bobby\Downloads\LINE_.txt";
            InitEmojiLookup();

            var currentDate = default(DateTime);
            using (var sr = new StreamReader(inputFilePath))
            {
                var parseTask = StartParsing();
                var lineModel = default(LineDataModel);
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var parts = line.Split('\t');

                    //New message Line
                    if (parts.Length == 3 && TimeSpan.TryParse(parts[0], out var timestamp))
                    {
                        var messageTimestamp = currentDate.Date.Add(timestamp);
                        lineModel = new LineDataModel();
                        lineModel.Timestamp = messageTimestamp;
                        lineModel.Sender = parts[1];
                        lineModel.Message = lineModel.Message += parts[2];
                        _messageParseQueue.Enqueue(lineModel);
                    }
                    //Could be in 2nd + line of message or new day
                    else
                    {
                        //Try to extract and parse a date
                        var date = line.Substring(0, line.Length - 3);
                        if (DateTime.TryParse(date, out var dt))
                        {
                            currentDate = dt;
                        }
                        //Middle of message
                        else
                        {
                            lineModel.Message += line;
                        }
                    }
                }

                _finishedReading = true;
                parseTask.GetAwaiter().GetResult();
            }
        }

        static Task StartParsing()
        {
            return Task.Run(() =>
            {
                while (!_finishedReading)
                {
                    if (_messageParseQueue.TryDequeue(out var currentMessage))
                    {
                        // Need to detect emojis and words and shit
                        var chars = currentMessage.Message.ToCharArray();
                        foreach (var c in chars)
                        {
                        }
                    }
                }
            });
        }

        static void InitEmojiLookup()
        {
            _emojiLookup.Add("😀", "Grinning Face");
            _emojiLookup.Add("😃", "Grinning Face with Big Eyes");
            _emojiLookup.Add("😄", "Grinning Face with Smiling Eyes");
            _emojiLookup.Add("😁", "Beaming Face with Smiling Eyes");
            _emojiLookup.Add("😆", "Grinning Squinting Face");
            _emojiLookup.Add("😅", "Grinning Face with Sweat");
            _emojiLookup.Add("🤣", "Rolling on the Floor Laughing");
            _emojiLookup.Add("😂", "Face with Tears of Joy");
            _emojiLookup.Add("🙂", "Slightly Smiling Face");
            _emojiLookup.Add("🙃", "Upside - Down Face");
            _emojiLookup.Add("😉", "Winking Face");
            _emojiLookup.Add("😊", "Smiling Face with Smiling Eyes");

            // Finish adding these guys
            //😇 Smiling Face with Halo
            //🥰 Smiling Face with Hearts
            //😍 Smiling Face with Heart-Eyes
            //🤩 Star - Struck
            //😘 Face Blowing a Kiss
            //😗 Kissing Face
            //☺️ Smiling Face
            //😚 Kissing Face with Closed Eyes
            //😙 Kissing Face with Smiling Eyes
            //🥲 Smiling Face with Tear
            //😋 Face Savoring Food
            //😛 Face with Tongue
            //😜 Winking Face with Tongue
            //🤪 Zany Face
            //😝 Squinting Face with Tongue
            //🤑 Money - Mouth Face
            //🤗 Smiling Face with Open Hands
            //🤭 Face with Hand Over Mouth
            //🤫 Shushing Face
            //🤔 Thinking Face
            //🤐 Zipper - Mouth Face
            //🤨 Face with Raised Eyebrow
            //😐 Neutral Face
            //😑 Expressionless Face
            //😶 Face Without Mouth
            //😶‍🌫️ Face in Clouds
            //😏 Smirking Face
            //😒 Unamused Face
            //🙄 Face with Rolling Eyes
            //😬 Grimacing Face
            //😮‍💨 Face Exhaling
            //🤥 Lying Face
            //😌 Relieved Face
            //😔 Pensive Face
            //😪 Sleepy Face
            //🤤 Drooling Face
            //😴 Sleeping Face
            //😷 Face with Medical Mask
            //🤒 Face with Thermometer
            //🤕 Face with Head - Bandage
            //🤢 Nauseated Face
            //🤮 Face Vomiting
            //🤧 Sneezing Face
            //🥵 Hot Face
            //🥶 Cold Face
            //🥴 Woozy Face
            //😵 Face with Crossed - Out Eyes
            //😵‍💫 Face with Spiral Eyes
            //🤯 Exploding Head
            //🤠 Cowboy Hat Face
            //🥳 Partying Face
            //🥸 Disguised Face
            //😎 Smiling Face with Sunglasses
            //🤓 Nerd Face
            //🧐 Face with Monocle
            //😕 Confused Face
            //😟 Worried Face
            //🙁 Slightly Frowning Face
            //☹️ Frowning Face
            //😮 Face with Open Mouth
            //😯 Hushed Face
            //😲 Astonished Face
            //😳 Flushed Face
            //🥺 Pleading Face
            //😦 Frowning Face with Open Mouth
            //😧 Anguished Face
            //😨 Fearful Face
            //😰 Anxious Face with Sweat
            //😥 Sad but Relieved Face
            //😢 Crying Face
            //😭 Loudly Crying Face
            //😱 Face Screaming in Fear
            //😖 Confounded Face
            //😣 Persevering Face
            //😞 Disappointed Face
            //😓 Downcast Face with Sweat
            //😩 Weary Face
            //😫 Tired Face
            //🥱 Yawning Face
            //😤 Face with Steam From Nose
            //😡 Enraged Face
            //😠 Angry Face
            //🤬 Face with Symbols on Mouth
            //😈 Smiling Face with Horns
            //👿 Angry Face with Horns
            //💀 Skull
            //☠️ Skull and Crossbones
            //💩 Pile of Poo
            //🤡 Clown Face
            //👹 Ogre
            //👺 Goblin
            //👻 Ghost
            //👽 Alien
            //👾 Alien Monster
            //🤖 Robot
            //😺 Grinning Cat
            //😸 Grinning Cat with Smiling Eyes
            //😹 Cat with Tears of Joy
            //😻 Smiling Cat with Heart-Eyes
            //😼 Cat with Wry Smile
            //😽 Kissing Cat
            //🙀 Weary Cat
            //😿 Crying Cat
            //😾 Pouting Cat
            //💋 Kiss Mark
            //👋 Waving Hand
            //🤚 Raised Back of Hand
            //🖐️ Hand with Fingers Splayed
            //✋ Raised Hand
            //🖖 Vulcan Salute
            //👌 OK Hand
            //🤌 Pinched Fingers
            //🤏 Pinching Hand
            //✌️ Victory Hand
            //🤞 Crossed Fingers
            //🤟 Love-You Gesture
            //🤘 Sign of the Horns
            //🤙 Call Me Hand
            //👈 Backhand Index Pointing Left
            //👉 Backhand Index Pointing Right
            //👆 Backhand Index Pointing Up
            //🖕 Middle Finger
            //👇 Backhand Index Pointing Down
            //☝️ Index Pointing Up
            //👍 Thumbs Up
            //👎 Thumbs Down
            //✊ Raised Fist
            //👊 Oncoming Fist
            //🤛 Left-Facing Fist
            //🤜 Right-Facing Fist
            //👏 Clapping Hands
            //🙌 Raising Hands
            //👐 Open Hands
            //🤲 Palms Up Together
            //🤝 Handshake
            //🙏 Folded Hands
            //✍️ Writing Hand
            //💅 Nail Polish
            //🤳 Selfie
            //💪 Flexed Biceps
            //🦾 Mechanical Arm
            //🦿 Mechanical Leg
            //🦵 Leg
            //🦶 Foot
            //👂 Ear
            //🦻 Ear with Hearing Aid
            //👃 Nose
            //🧠 Brain
            //🫀 Anatomical Heart
            //🫁 Lungs
            //🦷 Tooth
            //🦴 Bone
            //👀 Eyes
            //👁️ Eye
            //👅 Tongue
            //👄 Mouth
            //👶 Baby
            //🧒 Child
            //👦 Boy
            //👧 Girl
            //🧑 Person
            //👱 Person: Blond Hair
            //👨 Man
            //🧔 Person: Beard
            //👨‍🦰 Man: Red Hair
            //👨‍🦱 Man: Curly Hair
            //👨‍🦳 Man: White Hair
            //👨‍🦲 Man: Bald
            //👩 Woman
            //👩‍🦰 Woman: Red Hair
            //🧑‍🦰 Person: Red Hair
            //👩‍🦱 Woman: Curly Hair
            //🧑‍🦱 Person: Curly Hair
            //👩‍🦳 Woman: White Hair
            //🧑‍🦳 Person: White Hair
            //👩‍🦲 Woman: Bald
            //🧑‍🦲 Person: Bald
            //👱‍♀️ Woman: Blond Hair
            //👱‍♂️ Man: Blond Hair
            //🧓 Older Person
            //👴 Old Man
            //👵 Old Woman
            //🙍 Person Frowning
            //🙍‍♂️ Man Frowning
            //🙍‍♀️ Woman Frowning
            //🙎 Person Pouting
            //🙎‍♂️ Man Pouting
            //🙎‍♀️ Woman Pouting
            //🙅 Person Gesturing No
            //🙅‍♂️ Man Gesturing No
            //🙅‍♀️ Woman Gesturing No
            //🙆 Person Gesturing OK
            //🙆‍♂️ Man Gesturing OK
            //🙆‍♀️ Woman Gesturing OK
            //💁 Person Tipping Hand
            //💁‍♂️ Man Tipping Hand
            //💁‍♀️ Woman Tipping Hand
            //🙋 Person Raising Hand
            //🙋‍♂️ Man Raising Hand
            //🙋‍♀️ Woman Raising Hand
            //🧏 Deaf Person
            //🧏‍♂️ Deaf Man
            //🧏‍♀️ Deaf Woman
            //🙇 Person Bowing
            //🙇‍♂️ Man Bowing
            //🙇‍♀️ Woman Bowing
            //🤦 Person Facepalming
            //🤦‍♂️ Man Facepalming
            //🤦‍♀️ Woman Facepalming
            //🤷 Person Shrugging
            //🤷‍♂️ Man Shrugging
            //🤷‍♀️ Woman Shrugging
            //🧑‍⚕️ Health Worker
            //👨‍⚕️ Man Health Worker
            //👩‍⚕️ Woman Health Worker
            //🧑‍🎓 Student
            //👨‍🎓 Man Student
            //👩‍🎓 Woman Student
            //🧑‍🏫 Teacher
            //👨‍🏫 Man Teacher
            //👩‍🏫 Woman Teacher
            //🧑‍⚖️ Judge
            //👨‍⚖️ Man Judge
            //👩‍⚖️ Woman Judge
            //🧑‍🌾 Farmer
            //👨‍🌾 Man Farmer
            //👩‍🌾 Woman Farmer
            //🧑‍🍳 Cook
            //👨‍🍳 Man Cook
            //👩‍🍳 Woman Cook
            //🧑‍🔧 Mechanic
            //👨‍🔧 Man Mechanic
            //👩‍🔧 Woman Mechanic
            //🧑‍🏭 Factory Worker
            //👨‍🏭 Man Factory Worker
            //👩‍🏭 Woman Factory Worker
            //🧑‍💼 Office Worker
            //👨‍💼 Man Office Worker
            //👩‍💼 Woman Office Worker
            //🧑‍🔬 Scientist
            //👨‍🔬 Man Scientist
            //👩‍🔬 Woman Scientist
            //🧑‍💻 Technologist
            //👨‍💻 Man Technologist
            //👩‍💻 Woman Technologist
            //🧑‍🎤 Singer
            //👨‍🎤 Man Singer
            //👩‍🎤 Woman Singer
            //🧑‍🎨 Artist
            //👨‍🎨 Man Artist
            //👩‍🎨 Woman Artist
            //🧑‍✈️ Pilot
            //👨‍✈️ Man Pilot
            //👩‍✈️ Woman Pilot
            //🧑‍🚀 Astronaut
            //👨‍🚀 Man Astronaut
            //👩‍🚀 Woman Astronaut
            //🧑‍🚒 Firefighter
            //👨‍🚒 Man Firefighter
            //👩‍🚒 Woman Firefighter
            //👮 Police Officer
            //👮‍♂️ Man Police Officer
            //👮‍♀️ Woman Police Officer
            //🕵️ Detective
            //🕵️‍♂️ Man Detective
            //🕵️‍♀️ Woman Detective
            //💂 Guard
            //💂‍♂️ Man Guard
            //💂‍♀️ Woman Guard
            //🥷 Ninja
            //👷 Construction Worker
            //👷‍♂️ Man Construction Worker
            //👷‍♀️ Woman Construction Worker
            //🤴 Prince
            //👸 Princess
            //👳 Person Wearing Turban
            //👳‍♂️ Man Wearing Turban
            //👳‍♀️ Woman Wearing Turban
            //👲 Person with Skullcap
            //🧕 Woman with Headscarf
            //🤵 Person in Tuxedo
            //🤵‍♂️ Man in Tuxedo
            //🤵‍♀️ Woman in Tuxedo
            //👰 Person with Veil
            //👰‍♂️ Man with Veil
            //👰‍♀️ Woman with Veil
            //🤰 Pregnant Woman
            //🤱 Breast-Feeding
            //👩‍🍼 Woman Feeding Baby
            //👨‍🍼 Man Feeding Baby
            //🧑‍🍼 Person Feeding Baby
            //👼 Baby Angel
            //🎅 Santa Claus
            //🤶 Mrs. Claus
            //🧑‍🎄 Mx Claus
            //🦸 Superhero
            //🦸‍♂️ Man Superhero
            //🦸‍♀️ Woman Superhero
            //🦹 Supervillain
            //🦹‍♂️ Man Supervillain
            //🦹‍♀️ Woman Supervillain
            //🧙 Mage
            //🧙‍♂️ Man Mage
            //🧙‍♀️ Woman Mage
            //🧚 Fairy
            //🧚‍♂️ Man Fairy
            //🧚‍♀️ Woman Fairy
            //🧛 Vampire
            //🧛‍♂️ Man Vampire
            //🧛‍♀️ Woman Vampire
            //🧜 Merperson
            //🧜‍♂️ Merman
            //🧜‍♀️ Mermaid
            //🧝 Elf
            //🧝‍♂️ Man Elf
            //🧝‍♀️ Woman Elf
            //🧞 Genie
            //🧞‍♂️ Man Genie
            //🧞‍♀️ Woman Genie
            //🧟 Zombie
            //🧟‍♂️ Man Zombie
            //🧟‍♀️ Woman Zombie
            //💆 Person Getting Massage
            //💆‍♂️ Man Getting Massage
            //💆‍♀️ Woman Getting Massage
            //💇 Person Getting Haircut
            //💇‍♂️ Man Getting Haircut
            //💇‍♀️ Woman Getting Haircut
            //🚶 Person Walking
            //🚶‍♂️ Man Walking
            //🚶‍♀️ Woman Walking
            //🧍 Person Standing
            //🧍‍♂️ Man Standing
            //🧍‍♀️ Woman Standing
            //🧎 Person Kneeling
            //🧎‍♂️ Man Kneeling
            //🧎‍♀️ Woman Kneeling
            //🧑‍🦯 Person with White Cane
            //👨‍🦯 Man with White Cane
            //👩‍🦯 Woman with White Cane
            //🧑‍🦼 Person in Motorized Wheelchair
            //👨‍🦼 Man in Motorized Wheelchair
            //👩‍🦼 Woman in Motorized Wheelchair
            //🧑‍🦽 Person in Manual Wheelchair
            //👨‍🦽 Man in Manual Wheelchair
            //👩‍🦽 Woman in Manual Wheelchair
            //🏃 Person Running
            //🏃‍♂️ Man Running
            //🏃‍♀️ Woman Running
            //💃 Woman Dancing
            //🕺 Man Dancing
            //🕴️ Person in Suit Levitating
            //👯 People with Bunny Ears
            //👯‍♂️ Men with Bunny Ears
            //👯‍♀️ Women with Bunny Ears
            //🧖 Person in Steamy Room
            //🧖‍♂️ Man in Steamy Room
            //🧖‍♀️ Woman in Steamy Room
            //🧘 Person in Lotus Position
            //🧑‍🤝‍🧑 People Holding Hands
            //👭 Women Holding Hands
            //👫 Woman and Man Holding Hands
            //👬 Men Holding Hands
            //💏 Kiss
            //👩‍❤️‍💋‍👨 Kiss: Woman, Man
            //👨‍❤️‍💋‍👨 Kiss: Man, Man
            //👩‍❤️‍💋‍👩 Kiss: Woman, Woman
            //💑 Couple with Heart
            //👩‍❤️‍👨 Couple with Heart: Woman, Man
            //👨‍❤️‍👨 Couple with Heart: Man, Man
            //👩‍❤️‍👩 Couple with Heart: Woman, Woman
            //👪 Family
            //👨‍👩‍👦 Family: Man, Woman, Boy
            //👨‍👩‍👧 Family: Man, Woman, Girl
            //👨‍👩‍👧‍👦 Family: Man, Woman, Girl, Boy
            //👨‍👩‍👦‍👦 Family: Man, Woman, Boy, Boy
            //👨‍👩‍👧‍👧 Family: Man, Woman, Girl, Girl
            //👨‍👨‍👦 Family: Man, Man, Boy
            //👨‍👨‍👧 Family: Man, Man, Girl
            //👨‍👨‍👧‍👦 Family: Man, Man, Girl, Boy
            //👨‍👨‍👦‍👦 Family: Man, Man, Boy, Boy
            //👨‍👨‍👧‍👧 Family: Man, Man, Girl, Girl
            //👩‍👩‍👦 Family: Woman, Woman, Boy
            //👩‍👩‍👧 Family: Woman, Woman, Girl
            //👩‍👩‍👧‍👦 Family: Woman, Woman, Girl, Boy
            //👩‍👩‍👦‍👦 Family: Woman, Woman, Boy, Boy
            //👩‍👩‍👧‍👧 Family: Woman, Woman, Girl, Girl
            //👨‍👦 Family: Man, Boy
            //👨‍👦‍👦 Family: Man, Boy, Boy
            //👨‍👧 Family: Man, Girl
            //👨‍👧‍👦 Family: Man, Girl, Boy
            //👨‍👧‍👧 Family: Man, Girl, Girl
            //👩‍👦 Family: Woman, Boy
            //👩‍👦‍👦 Family: Woman, Boy, Boy
            //👩‍👧 Family: Woman, Girl
            //👩‍👧‍👦 Family: Woman, Girl, Boy
            //👩‍👧‍👧 Family: Woman, Girl, Girl
            //🗣️ Speaking Head
            //👤 Bust in Silhouette
            //👥 Busts in Silhouette
            //🫂 People Hugging
            //👣 Footprints
            //🧳 Luggage
            //🌂 Closed Umbrella
            //☂️ Umbrella
            //🎃 Jack-O-Lantern
            //🧵 Thread
            //🧶 Yarn
            //👓 Glasses
            //🕶️ Sunglasses
            //🥽 Goggles
            //🥼 Lab Coat
            //🦺 Safety Vest
            //👔 Necktie
            //👕 T-Shirt
            //👖 Jeans
            //🧣 Scarf
            //🧤 Gloves
            //🧥 Coat
            //🧦 Socks
            //👗 Dress
            //👘 Kimono
            //🥻 Sari
            //🩱 One-Piece Swimsuit
            //🩲 Briefs
            //🩳 Shorts
            //👙 Bikini
            //👚 Woman’s Clothes
            //👛 Purse
            //👜 Handbag
            //👝 Clutch Bag
            //🎒 Backpack
            //🩴 Thong Sandal
            //👞 Man’s Shoe
            //👟 Running Shoe
            //🥾 Hiking Boot
            //🥿 Flat Shoe
            //👠 High-Heeled Shoe
            //👡 Woman’s Sandal
            //🩰 Ballet Shoes
            //👢 Woman’s Boot
            //👑 Crown
            //👒 Woman’s Hat
            //🎩 Top Hat
            //🎓 Graduation Cap
            //🧢 Billed Cap
            //🪖 Military Helmet
            //⛑️ Rescue Worker’s Helmet
            //💄 Lipstick
            //💍 Ring
            //💼 Briefcase
            //🩸 Drop of Blood
        }
    }
}
