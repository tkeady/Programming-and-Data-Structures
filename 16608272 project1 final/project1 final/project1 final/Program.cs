using System;
using System.IO;
class textAnalyse
{

    public void readText()
    {
        Console.WriteLine("Enter text, end each sentence with  * (asterisk)"); // User enters text to be saved in string
        string userEnteredText = Console.ReadLine();

        File.AppendAllText("TextEntry.txt", userEnteredText); // Adds entered text to new file then reads said file
        string readEnteredText = File.ReadAllText("TextEntry.txt");

        // User confirmation
        Console.WriteLine("Finished?\nInput 'Yes' if done or 'No' if not");

        // Save entry as string and transfers to switch statement
        string userChoice = Console.ReadLine();
        string userChoiceLowered = userChoice.ToLower();

        // Check if user is done
        switch (userChoiceLowered)
        {
            // If no run readText() again
            case "no":
                readText();
                break;

            case "yes": // basicResponse() gives analysis on 'yes'
                Console.WriteLine("Sentence analysis:");
                basicResponse(readEnteredText);
                break;

            default: // Default input assumes yes
                Console.WriteLine("Default = yes");
                Console.WriteLine("Sentence analysis:");
                basicResponse(readEnteredText);
                break;
        }
    }

    public void basicResponse(string enteredText)
    {
        char[] enteredTextArray = enteredText.ToCharArray(); // Text is enteredText and converts to char array for statements 

        // Commands get own string from defined function
        Console.WriteLine("Number of sentences entered: {0}", returnsentence(enteredTextArray));
        Console.WriteLine("Number of vowels: {0}", returnVowel(enteredTextArray));
        Console.WriteLine("Number of consonants: {0}", returnConsonant(enteredTextArray));
        Console.WriteLine("Number of upper case letters: {0}", returnUpper(enteredTextArray));
        Console.WriteLine("Number of lower case letters: {0}", returnLower(enteredTextArray));

        Console.WriteLine("Would you like to see the frequency of an specific letter?"); // Ask user if they want the letter frequency
        string userChoiceLetterFrequency = Console.ReadLine().ToLower();
        switch (userChoiceLetterFrequency)
        {

            case "yes": // If yes use returnIndividualLetters()
                returnIndividualLetters(enteredTextArray);
                break;

            case "no": // If no close program and run deleteTextFile
                deleteTextFile();
                break;

            default: // Assume no and use deleteTextFile and close program
                deleteTextFile();
                break;
        }
    }

    public void returnIndividualLetters(char[] enteredTextString)
    { // Retrieve letter frequency
        Console.WriteLine("Enter the letter you want the frequency of:");

        try
        { // Get selected letter
            string userLetter = Console.ReadLine().ToLower();
            int userLetterAmount = 0; // foreach will find all characters and single out the specific letter
            foreach (char i in enteredTextString)
            {
                if (i.ToString().ToLower() == userLetter)
                {
                    userLetterAmount++; // Add up the total occurence of the letter
                }
            }


            Console.WriteLine("Letter {0} found {1} times", userLetter.ToUpper(), userLetterAmount);
            deleteTextFile();
        }
        catch (Exception e)
        {
            Console.WriteLine("Unexpected Input error {0}", e);
            deleteTextFile();
        }

    }

    public string returnsentence(char[] enteredTextString)
    {  // Return sentence amount using asterisk*
        int sentenceAmount = 0;
        foreach (char i in enteredTextString)
        { // Check every character
            switch (i)
            {
                case '*':
                    //If it is a * then increment the sentenceAmount count
                    sentenceAmount++;
                    break;
                default:
                    break;
            }
        }
        if (sentenceAmount == 0)
        {
            sentenceAmount++;
        }
        //Return the sentenceAmount in string form for Console to Write
        return sentenceAmount.ToString();
    }

    //Return the amount of vowels in the text
    public string returnVowel(char[] enteredTextString)
    {
        int vowelAmount = 0;

        //Check every character in the string
        foreach (char i in enteredTextString)
        {
            // vowelAmount incrementation method
            if ("AEIOUaeiou".Contains(i.ToString()))
            { // Vowel list
                vowelAmount++;
            }
        }
        return vowelAmount.ToString(); // vowelAmount to string for console
    }

    public string returnConsonant(char[] enteredTextString)
    { // Return consonants
        char[] consonantList = { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', 'Z', 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'y', 'z' }; // Consonant list
        int consonantAmount = 0;
        // For loop inside foreach loop to check char.
        foreach (char i in enteredTextString)
        { // Every found consonant identified as a match which adds to the consonant count
            for (int j = 0; j < consonantList.Length; j++)
            {
                if (i == consonantList[j])
                {
                    consonantAmount++; // Consonant increments
                }
            }
        }
        return consonantAmount.ToString();
    }

    public string returnUpper(char[] enteredTextString)
    { // Uppercase letter return
        int upperAmount = 0;
        foreach (char i in enteredTextString)
        {
            if (char.IsUpper(i))
            { // IsUpper finds uppercase letters and adds to total
                upperAmount++;
            }
        }
        return upperAmount.ToString();
    }

    public string returnLower(char[] enteredTextString)
    { // Lowercase return same as above
        int lowerAmount = 0;
        foreach (char i in enteredTextString)
        {
            if (char.IsLower(i))
            { // Used IsLower similarly to IsUpper
                lowerAmount++;
            }
        }
        return lowerAmount.ToString();
    }

    public string returnLongWords(string enteredTextString)
    { // For finding 'long words'
        string[] enteredTextConvert;
        enteredTextConvert = enteredTextString.Split(' ');

        int longWordAmount = 0; // Amount set to 0
        foreach (string i in enteredTextConvert)
        {
            if (i.Length > 7)
            { // If length of word (string in array) is more than seven characters then 1 is added 
                longWordAmount++;
                File.AppendAllText("longWord.txt", i + Environment.NewLine);
            }
        }

        return longWordAmount.ToString(); // For console to write
    }

    public void readFile()   // Option 2 to read text file
    {
        // Prompt user to enter file location
        Console.WriteLine(" Enter file path of the .txt file\nIf you input 'default' an example file will run");
        Console.Write("File path: ");
        string userFilePath = Console.ReadLine();
        switch (userFilePath)
        {
            case "default": // Opens the .txt from Blackboard about Yahoo
                string defaultFilePath = "text.txt";
                string[] defaultFileRead = File.ReadAllLines(defaultFilePath);
                string defaultFileString = string.Join("", defaultFileRead); // Needs to be string like earlier
                Console.WriteLine("Found {0} word(s) with a length greater than 7!", returnLongWords(defaultFileString));
                basicResponse(defaultFileString); // basicResponse function
                break;
            default:
                try
                {
                    string[] userFileRead = File.ReadAllLines(userFilePath); // Reads the .txt file from the user's file path
                    string userFileString = string.Join("", userFileRead); // Again needs to be a string
                    Console.WriteLine("Found {0} words with a length greater than 7", returnLongWords(userFileString));
                    basicResponse(userFileString);

                }
                catch (Exception e)
                {
                    Console.WriteLine("\nRead file error - Is the file location correct, add .txt onto end of file name?\n Returning to previous input"); // An error will let the user try again
                    readFile();
                }
                break;
        }
    }

    public void deleteTextFile()
    { // deleteTextFile function
        File.Delete("TextEntry.txt"); // Delete temporary text file
    }

    public static void Main()
    { // So that functions and method can be found
        textAnalyse textAnalysis = new textAnalyse();

        Console.WriteLine("Do you want to enter text or read text from a .txt file?"); //Introduction
        Console.WriteLine("1. Enter the text with keyboard \n2. Read text from a .txt file");

        try
        { // Checking if user did 1 or 2 and the subsequent process
            int keyboardOrFile = Convert.ToInt32(Console.ReadLine());
            if (keyboardOrFile == 1)
            {
                textAnalysis.readText();
            }
            else if (keyboardOrFile == 2)
            {
                textAnalysis.readFile();
            }
        }
        catch (Exception e)
        { // Prevents crash and closes program
            Console.WriteLine("Invalid");
        }

    }
} // 16608272 Thomas Keady