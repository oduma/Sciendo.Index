Feature: IndexingOfMusicFiles
In order to be able to search for my music
I want to be able to speficy what music 
files I should index

@musicFileIndexation
Scenario: Index a new music file on demand
	Given The file 'C:\Code\m\Music\S\Mr. Scruff\Ninja Tuna\Kalimba.mp3' is not indexed yet
	When I call the indexOnDemandService
	Then the result should be 1
