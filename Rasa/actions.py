from typing import Any, Text, Dict, List

from rasa_sdk import Action, Tracker
from rasa_sdk.executor import CollectingDispatcher


class ActionPokemonDescription(Action):

    def name(self) -> Text:
        return "action_tell_pokemon_description"

    def run(self, dispatcher: CollectingDispatcher,
            tracker: Tracker,
            domain: Dict[Text, Any]) -> List[Dict[Text, Any]]:
        selected_pokemon = tracker.get_slot("selected_pokemon")
        dispatcher.utter_message(text=f"The selected pokemon is {selected_pokemon}")
        return []
