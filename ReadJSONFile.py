import json
import OriginalData

global jsonFileData

#initialize original data
def init():
    ReadJSONFile()
    ReturnFinalStatesList()
    ReturnInitialStateString()
    ReturnTransitionsDictionary()
    ReturnFinalAndNonfinalStatesList()
    ReturnInputSymbolsList()

#open and read contents of input.json 
def ReadJSONFile():

    jsonFilePath = "input2.json"
    jsonFile = open(jsonFilePath)
    global jsonFileData
    jsonFileData = json.load(jsonFile)

#seperate final states
def ReturnFinalStatesList():
    answer = []
    for finalState in eval(jsonFileData["final_states"]):
        answer.append(finalState)
    OriginalData.final_states = answer

#seperate initial state
def ReturnInitialStateString():
    OriginalData.initial_state = jsonFileData["initial_state"]

# an example of return answer :
# {
#   q0 :
#   {
#       "a" : { "q1" , "q2 "}
#    }
# }

#seperate transitions
def ReturnTransitionsDictionary():
    transitions = jsonFileData["transitions"]

    answer = {}

    for state in transitions.keys():
        t = {}
        for alphabet in transitions[state].keys():
          #      t[alphabet] = eval(transitions[state][alphabet])
            t[alphabet] = []
            for i in eval(transitions[state][alphabet]):
                t[alphabet].append(i)
        answer[state] = t

    OriginalData.originalTransitions = answer

#seperate all states
def ReturnFinalAndNonfinalStatesList():
    OriginalData.originalStates = eval(jsonFileData["states"])

#seperate input symbols
def ReturnInputSymbolsList():
    OriginalData.input_symbols = eval(jsonFileData["input_symbols"])
