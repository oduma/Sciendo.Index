Feature: AcquireLyrics
In order to be able to Index a new lyrics file
I want to get the lyrics and save them locally
@AcquireLyricsForMusic
Scenario: Acquire Lyrics for a music file
	Given I have no lyrics file 'C:\Code\m\Lyrics\S\Mr. Scruff\Ninja Tuna\Kalimba.lrc'
	When I acquire the lyrics for 'C:\Code\m\Music\S\Mr. Scruff\Ninja Tuna\Kalimba.mp3'
	Then the result is the file 'C:\Code\m\Lyrics\S\Mr. Scruff\Ninja Tuna\Kalimba.lrc' exists
	And the result should be 1
