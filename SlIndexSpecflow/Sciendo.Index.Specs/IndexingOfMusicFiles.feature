Feature: IndexingOfMusicFiles
In order to be able to search for my music
I want to be able to speficy what music 
files I should index

@musicFileIndexation
Scenario: Index a new music file on demand
	Given The file 'c:\m\m.mp3' exists
	When I call the indexOnDemandService
	Then the result should be 1
