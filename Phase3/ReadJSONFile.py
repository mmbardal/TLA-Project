import json
import OriginalData

def init():
    ReadJSONFile()
    SetFinalStatesAsList()
    SetInitialStateAsString()
    SetTransitionsDictionary()
    SetStatesList()
    SetInputSymbolsList()

def ReadJSONFile():

    jsonFilePath = "input1.json"
    jsonFile = open(jsonFilePath)

    global jsonFileData
    jsonFileData = json.load(jsonFile)

def SetFinalStatesAsList():
    answer = []
    for item in eval(jsonFileData["final_states"]):
        answer.append(item)
    OriginalData.final_states = answer

def SetInitialStateAsString():
    OriginalData.initial_state = jsonFileData["initial_state"]

def SetTransitionsDictionary():
    transitions = jsonFileData["transitions"]

    answer = {}

    for state in transitions.keys():
        t = {}
        for alphabet in transitions[state].keys():
            t[alphabet] = []
            for i in eval(transitions[state][alphabet]):
                t[alphabet].append(i)
        answer[state] = t

    OriginalData.originalTransitions = answer

def SetStatesList():
    OriginalData.originalStates = eval(jsonFileData["states"])

def SetInputSymbolsList():
    OriginalData.input_symbols = eval(jsonFileData["input_symbols"])
