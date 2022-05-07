using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SoundsController : MonoBehaviour {
   public AudioSource audioSource;
   public Sounds sounds;

   public Text bgmText;
   public Slider bgmSlider;
   public Image bgmImg; 

   private AudioClip _audioClip;
   private Image _bmgSliderFillImg;
   private void Start() {
      _bmgSliderFillImg = bgmSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
      
      bgmSlider.value = 0;
      bgmImg.CrossFadeAlpha(.0f, .0f, false);
      _bmgSliderFillImg.CrossFadeAlpha(.0f, .0f, false);
      bgmText.CrossFadeAlpha(.0f, .0f, false);
      
      PlayBGM();
   }

   private void Update() {
      if (audioSource.isPlaying) {
         bgmSlider.value = audioSource.time / _audioClip.length;
         bgmImg.rectTransform.Rotate(new Vector3(0, 0, -0.05f));
      }
   }

   private void PlayBGM() {
      bgmSlider.value = 0;
         
      Random.InitState((int)System.DateTime.Now.Ticks);  // set seed
      _audioClip = sounds.bgm[Random.Range(0, sounds.bgm.Count - 1)];
      audioSource.clip = _audioClip;
      bgmText.text = _audioClip.name + ".ogg";
      
      audioSource.Play();
      
      bgmImg.CrossFadeAlpha(1.0f, 5.0f, false);
      _bmgSliderFillImg.CrossFadeAlpha(1.0f, 5.0f, false);
      bgmText.CrossFadeAlpha(1.0f, 5.0f, false);
      StartCoroutine(SchedulePlay());
   }
   
   private IEnumerator SchedulePlay() {
      yield return new WaitForSeconds(_audioClip.length - 3);
      bgmImg.CrossFadeAlpha(.0f, 5.0f, false);
      _bmgSliderFillImg.CrossFadeAlpha(.0f, 5.0f, false);
      bgmText.CrossFadeAlpha(.0f, 5.0f, false);
      
      yield return new WaitForSeconds(Random.Range(10, 30));
      PlayBGM();
   }

   public void PlayAudioClip(BlockType blockType, Vector3 pos, PlayerBehaviour playerBehaviour) {
          // wood
      if (Block.IsBlockInRange(blockType, Block.OfWood)) {
         AudioSource.PlayClipAtPoint(sounds.block[9].blockPlaceAndDigSound[Random.Range(0, sounds.block[9].blockPlaceAndDigSound.Count - 1)], pos);
      }  // glass
      else if(Block.IsBlockInRange(blockType, Block.OfGlass)) {
         switch (playerBehaviour) {
            case PlayerBehaviour.Dig:
               AudioSource.PlayClipAtPoint(sounds.block[10].blockBreakSound[Random.Range(0, sounds.block[10].blockBreakSound.Count - 1)], pos);
               break;
            default:
               AudioSource.PlayClipAtPoint(sounds.block[8].blockPlaceAndDigSound[Random.Range(0, sounds.block[8].blockPlaceAndDigSound.Count - 1)], pos);
               break;
         }
      }  // plant, leaf, grass, flower, grass_block, dirt
      else if (Block.IsBlockInRange(blockType, Block.CanPlant) || Block.IsBlockInRange(blockType, Block.OfLeaf) || blockType is BlockType.GrassBlock or BlockType.Dirt) {
         AudioSource.PlayClipAtPoint(sounds.block[2].blockPlaceAndDigSound[Random.Range(0, sounds.block[2].blockPlaceAndDigSound.Count - 1)], pos);
      }  // sand, gravel
      else if (blockType is BlockType.Sand or BlockType.Gravel) {
         AudioSource.PlayClipAtPoint(sounds.block[5].blockPlaceAndDigSound[Random.Range(0, sounds.block[5].blockPlaceAndDigSound.Count - 1)], pos);
      }
      else {
         AudioSource.PlayClipAtPoint(sounds.block[8].blockPlaceAndDigSound[Random.Range(0, sounds.block[8].blockPlaceAndDigSound.Count - 1)], pos);
      }
   }
}
