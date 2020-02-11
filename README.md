# Rasa-Unity
This is a tutorial to show how one can integrate Rasa chatbots with Unity.

![](https://github.com/retrogeek46/Rasa-Unity/blob/master/Resources/sample.gif)

Currently no work on chatbot has been done except implementing the custom connector. Chatbot is trained on the Mood bot data that is created after `rasa init` command.

## Installation
First, clone this repo using `git clone https://github.com/retrogeek46/Rasa-Unity` and checkout to v0.1.0.
### Rasa
- Change directory into Rasa/ and create a virtual environemt using  
`python -m venv venv`
- Activate the virtual environment  
Bash on Windows&nbsp;: `source venv/Scripts/activate`  
CMD : `venv\Scripts\activate`  
Linux : `source venv/bin/activate`
- Install Rasa into the virtual environment (elevated prompt recommended for creating spacy link)  
`pip install rasa[spacy]`  
`python -m spacy download en_core_web_md`  
`python -m spacy link en_core_web_md en`
- Run rasa server using  
`rasa run --debug`
### Unity
- The Unity project was made using 2019.3 hence is the recommended version.
- Open Unity Hub and choose "Add" from top right corner.
- Select the Unity project folder located in Rasa-Unity/Unity/Rasa.
- Press play __after__ running the Rasa server.
- Type your message and the bot should respond.

## Docs
__WIP__, the source code is commented though.

## TODO
1. Add scroll support
2. Resize chat bubbles based on size of content (text, image)
3. Render buttons and quick replies
4. Add support for custom data
