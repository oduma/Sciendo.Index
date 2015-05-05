Feature: AcquireLyrics
In order to be able to Index a new lyrics file
I want to get the lyrics and save them locally
@AcquireLyricsForMusic
Scenario: Acquire Lyrics for a music file
	Given I have no lyrics file 'C:\Code\m\Lyrics\B\BoneyM\BoneyM Greatest Hits\Brown girl in the ring.lrc'
	When I acquire the lyrics for 'C:\Code\m\Music\B\BoneyM\BoneyM Greatest Hits\Brown girl in the ring.mp3'
	Then the result is the file 'C:\Code\m\Lyrics\B\BoneyM\BoneyM Greatest Hits\Brown girl in the ring.lrc' exists
	And the result should be 1
