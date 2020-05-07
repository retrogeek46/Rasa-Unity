# Rasa-Unity
This is the source code for integrating Rasa chatbots with Unity. Find the tutorial [here](https://medium.com/@divyangpradeep/integrating-rasa-open-source-chatbot-into-unity-part-1-the-connection-9ba582c804cd)

## Installation
First, clone this repo using `git clone https://github.com/retrogeek46/Rasa-Unity` and checkout to v0.2.0.
 ### Rasa
- Change directory into Rasa/ and create a virtual environment using  
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
`rasa run`
### Unity
- The Unity project was made using 2019.3 hence is the recommended version.
- Open Unity Hub and choose "Add" from top right corner.
- Select the Unity project folder located in Rasa-Unity/Unity/Rasa.
- Press play __after__ running the Rasa server.
- Type your message and the bot should respond.
