from typing import Any, Text, Dict, List

from rasa_sdk import Action, Tracker
from rasa_sdk.events import UserUtteranceReverted
from rasa_sdk.executor import CollectingDispatcher

from .pokemon import Pokemon

class ActionPokemonDescription(Action):

    def name(self) -> Text:
        return "action_tell_pokemon_description"

    def run(self, dispatcher: CollectingDispatcher,
            tracker: Tracker,
            domain: Dict[Text, Any]) -> List[Dict[Text, Any]]:

        # get pokemon name
        selected_pokemon = tracker.get_slot("selected_pokemon")

        # get pokemon description
        pokemon = Pokemon()
        description = pokemon.description(selected_pokemon)

        # show message to user
        dispatcher.utter_message(text=f"{selected_pokemon.upper()} : " + description)

        return []


class ActionPokemonQuery(Action):

    def name(self) -> Text:
        return "action_pokemon_query"

    def run(self, dispatcher: CollectingDispatcher,
            tracker: Tracker,
            domain: Dict[Text, Any]) -> List[Dict[Text, Any]]:

        # get pokemon name and query
        selected_pokemon = tracker.get_slot("selected_pokemon")
        query = tracker.get_slot("query")

        # get pokemon description
        pokemon = Pokemon()
        extra_info = pokemon.extra_info(selected_pokemon, query)

        # show message to user
        dispatcher.utter_message(text=extra_info)
        return [UserUtteranceReverted()]


class ActionDefaultFallback(Action):
    """Mapped action for fallback"""

    def name(self):
        return "action_default_fallback"

    def run(self, dispatcher: CollectingDispatcher,
            tracker: Tracker,
            domain: Dict[Text, Any]) -> List[Dict[Text, Any]]:

        dispatcher.utter_message(text="Sorry I didn't understand that")

        return []