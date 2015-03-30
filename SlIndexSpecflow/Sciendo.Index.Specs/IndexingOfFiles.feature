Feature: IndexingOfFiles
In order to be able to search for my music
I want to be able to speficy what music 
files I should index

@musicFileIndexation
Scenario: Index a new music file on demand
	Given The music file 'C:\Code\m\Music\B\BoneyM\BoneyM Greatest Hits\Brown girl in the ring.mp3' is not indexed yet
	When I call the indexOnDemandService for Music
	Then the result should be 1

@lyricsFileIndexation
Scenario: Index a new lyrics file on demand
	Given The lyrics file 'C:\Code\m\Music\B\BoneyM\BoneyM Greatest Hits\Brown girl in the ring.mp3' is not indexed yet
	When I call the indexOnDemandService for Lyrics 'C:\Code\m\Lyrics\B\BoneyM\BoneyM Greatest Hits\Brown girl in the ring.lrc'
	Then the result should be 1

Scenario: Index a new music folder on demand
	Given None of the files in folder 'C:\Code\m'
