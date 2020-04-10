"""
Data about the pokemon
"""

class Pokemon:

    def description(self, selected_pokemon):

        description_dict = {
            "Charizard": "It spits fire that is hot enough to melt boulders. It may cause forest fires by blowing flames.",
            "Rayquaza": "It has lived for hundreds of millions of years in the ozone layer. Its flying form looks like a meteor.",
            "Geodude": "Found in fields and mountains. Mistaking them for boulders, people often step or trip on them.",
            "Pikachu": "When several of these POKéMON gather, their electricity can build and cause lightning storms.",
            "Lapras": "A gentle soul that can read the minds of people. It can ferry people across the sea on its back."
        }

        return description_dict.get(selected_pokemon, f"Could Not find {selected_pokemon} in {str(description_dict.keys())}")

    def extra_info(self, selected_pokemon, query):

        evolution_dict = {
            "Charizard": "Charmander [LV 16]-> Charmeleon [LV 36]-> Charizard",
            "Rayquaza": "Rayquaza is a legendary Pokemon, it has no evolution tree",
            "Geodude": "Geodude [LV 25]-> Graveler [TRADE]-> Golem",
            "Pikachu": "Pichu [HIGH FRIENDSHIP]-> Pikachu [THUNDERSTONE]-> Raichu",
            "Lapras": "Lapras has no evolution tree"
        }

        type_dict = {
            "Charizard": "FIRE/FLYING",
            "Rayquaza": "DRAGON/FLYING",
            "Geodude": "ROCK/GROUND",
            "Pikachu": "ELECTRIC",
            "Lapras": "WATER/ICE"
        }

        region_dict = {
            "Charizard": "It is a started pokemon found in games such as Red/Blue and FireRed/LeafGreen",
            "Rayquaza": "It is a Legendary Pokemon found on top of Mt in Emerald",
            "Geodude": "It is found in fields and mountains.",
            "Pikachu": "It is found in Safari Zone and Viridian forest (if lucky)",
            "Lapras": "It can be found in Silph Co. and Icefall Cave"
        }

        extra_info_dict = {
            "evolution": evolution_dict,
            "region": region_dict,
            "type": type_dict
        }

        return extra_info_dict.get(query, f"Could not get {query} in {str(extra_info_dict.keys())}").get(selected_pokemon, f"Could not get {selected_pokemon}")