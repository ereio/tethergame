using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class MenuUiInteraction : MonoBehaviour {

	public UISprite spritePlay, spriteOptions, spriteQuit,
					spriteTitle;
	public Texture playNormal, playSelected;
	public Texture optionsNormal, optionsSelected;
	public Texture quitNormal, quitSelected;
	public UIPanel panelBackground;

	private int PLAY = 2;
	private int OPTIONS = 1;
	private int QUIT = 0;
	private int _current_item = 2;
	private bool _return_pressed = false;


	// Use this for initialization
	void Start () {
		_current_item = 2;
		StartCoroutine(loadMenuAnimation());
	}
	
	// Update is called once per frame
	void Update () {


		if(Input.GetKeyDown(KeyCode.UpArrow)) _current_item++;
		if(Input.GetKeyDown(KeyCode.DownArrow)) _current_item--;
		if(Input.GetKeyDown (KeyCode.Return) && !_return_pressed){
			StartCoroutine(fillProgressBar());
			_return_pressed = true;
		}

		_current_item = _current_item < QUIT ? PLAY : _current_item;
		_current_item %= 3;

		setHighlightPlay (_current_item == PLAY);
		setHighlightOptions (_current_item == OPTIONS);
		setHighlightQuit (_current_item == QUIT);
	}

	IEnumerator loadMenuAnimation(){
		while(!_return_pressed){
			Vector3 play_position = new Vector3(0, 0.05f, 0);
			Vector3 back_playpostion = new Vector3 (0, -0.05f, 0);
			float time = 4;

			Sequence seq = new Sequence();
			seq.Append(HOTween.To(spritePlay.transform,time, "position", play_position, true));
			seq.Append(HOTween.To(spritePlay.transform,time, "position", back_playpostion, true));
			seq.Play();

			yield return new WaitForSeconds (0.25f);

			Sequence seq2 = new Sequence();
			seq2.Append(HOTween.To(spriteTitle.transform,time, "position", play_position, true));
			seq2.Append(HOTween.To(spriteTitle.transform,time, "position", back_playpostion, true));
			seq2.Play();

			yield return new WaitForSeconds (0.25f);

			Sequence seq3 = new Sequence();
			seq3.Append(HOTween.To(spriteOptions.transform,time, "position", play_position, true));
			seq3.Append(HOTween.To(spriteOptions.transform,time, "position", back_playpostion, true));
			seq3.Play();

			yield return new WaitForSeconds (0.25f);

			Sequence seq4 = new Sequence(new SequenceParms());
			seq4.Append(HOTween.To(spriteQuit.transform,time, "position", play_position, true));
			seq4.Append(HOTween.To(spriteQuit.transform,time, "position", back_playpostion, true));
			seq4.Play();

			yield return new WaitForSeconds(8f);
		}
			
	}

	IEnumerator fillProgressBar(){
		while(getProgressValue() > 0){
			if(getProgressValue() > 0.5){
				addProgress(-0.02f);
				panelBackground.alpha += 0.02f;
				yield return new WaitForSeconds(0.025f);
			} else {
				addProgress(-0.05f);
				panelBackground.alpha += 0.02f;
				yield return new WaitForSeconds(0.02f);
			}
		}

		yield return new WaitForSeconds (2f);
		handleEnterHit ();
		yield return null;
	}

	void handleEnterHit(){
		switch(_current_item){
			case 0:
				Application.Quit();
				break;
			case 1:
				// options menu
				break;
			case 2:
				Application.LoadLevel("MainIsland");
				break;
		}
	}

	void setHighlightPlay(bool status){
		if(status){
			spritePlay.spriteName = playSelected.name;
			spriteQuit.spriteName = quitNormal.name;
			spriteOptions.spriteName = optionsNormal.name;
		} else {
			spritePlay.spriteName = playNormal.name;
		}
	}

	void setHighlightQuit(bool status){
		if(status){
			spritePlay.spriteName = playNormal.name;
			spriteQuit.spriteName = quitSelected.name;
			spriteOptions.spriteName = optionsNormal.name;
		} else {
			spriteQuit.spriteName = quitNormal.name;
		}
	}

	void setHighlightOptions(bool status){
		if(status){
			spritePlay.spriteName = playNormal.name;
			spriteQuit.spriteName = quitNormal.name;
			spriteOptions.spriteName = optionsSelected.name;
		} else {
			spriteOptions.spriteName = optionsNormal.name;
		}
	}

	void setProgressValue(float value){
		spritePlay.fillAmount = value;
		spriteTitle.fillAmount = value;
		spriteQuit.fillAmount = value;
		spriteOptions.fillAmount = value;
	}

	float getProgressValue(){
		return spritePlay.fillAmount;
	}

	void addProgress(float delta){
		spriteTitle.fillAmount += delta;
		spritePlay.fillAmount += delta;
		spriteOptions.fillAmount += delta;
		spriteQuit.fillAmount += delta;

	}
}
