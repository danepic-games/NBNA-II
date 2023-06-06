using System;
using System.Linq;
using System.Text.RegularExpressions;
using SerializableHelper;
using UnityEditor;
using UnityEngine;

public class InteractionComposerEditor : EditorWindow {

    private GameObject selectedGameObject;

    private SpriteRenderer spriteRenderer;
    private AbstractDataController abstractDataController;
    private AbstractDataController activeAbstractDataController;

    public ItrKindEnum kind;
    public float x;
    public float y;
    public float z;
    public float w;
    public float h;
    public float zwidth;
    public float dvx;
    public float dvy;
    public float dvz;

    public float arest;
    public float vrest;

    public int action;
    public int power;
    public bool defensable;
    public int injury;

    public AudioClip sound;
    public float confuse;
    public float silence;
    public float slow;
    public float stun;
    public float ignite;
    public float poison;
    public float root;
    public float charm;
    public float fear;
    public float taunt;
    public float blind;
    public float paralysis;
    public float freeze;

    private int idSelectionSearch;
    private int frameIdSelected;

    private InteractionData itrData;
    private InteractionData itrData2;
    private InteractionData itrData3;
    private ComposerElementsNumberEnum itrNumber;
    private bool loadItr1ByComposer;
    private bool loadItr2ByComposer;
    private bool loadItr3ByComposer;

    private Map<int, Sprite> sprites;
    private Map<int, FrameData> frames;
    private FrameData selectedFrame;


    [MenuItem("Frame/Itr Composer")]
    public static void Init() {
        var window = GetWindow<OpointComposerEditor>("Itr Composer");
    }

    void OnGUI() {
        EditorGUILayout.BeginHorizontal();
        this.selectedGameObject = (GameObject)EditorGUILayout.ObjectField("Object Data Controller", this.selectedGameObject, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        if (this.selectedGameObject && !this.selectedGameObject.TryGetComponent<SpriteRenderer>(out this.spriteRenderer)) {
            throw new NullReferenceException("Sprite Renderer for LF2 Object Builder must not be null");
        }
        if (this.selectedGameObject && !this.selectedGameObject.TryGetComponent<AbstractDataController>(out this.abstractDataController)) {
            throw new NullReferenceException("Data Controller for LF2 Object Builder must not be null");
        }

        if (this.selectedGameObject && this.spriteRenderer && this.abstractDataController) {
            this.BuildView();
        }
    }

    private void BuildView() {
        if (activeAbstractDataController == null || activeAbstractDataController.gameObject.GetInstanceID() != selectedGameObject.GetInstanceID()) {
            Debug.Log("Objeto Trocado.");
            this.frames = new Map<int, FrameData>();
        }

        if (this.frames.Count == 0) {
            activeAbstractDataController = this.selectedGameObject.GetComponent<AbstractDataController>();
            this.PopulateFramesOfObject();
        }
        if (this.frames.Count > 0) {

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Frame to Edit");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            idSelectionSearch = EditorGUILayout.IntField("Search Frame", idSelectionSearch);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("<")) {
                idSelectionSearch--;
            }
            if (GUILayout.Button(">")) {
                idSelectionSearch++;
            }

            var frameIds = this.frames.Where(frame => frame.Key == idSelectionSearch)
            .Select(frame => frame.Key).ToArray();

            EditorGUILayout.EndHorizontal();

            if (frameIds.Length == 1) {
                frameIdSelected = frameIds[0];
                selectedFrame = this.frames[frameIdSelected];
                this.SetFrameSprite();
                this.DrawComposerItrs();
                this.BuildItr();
            }

            EditorGUILayout.Separator();
        }
    }

    void OnHierarchyChange() {
        Repaint();
    }

    private void PopulateFramesOfObject() {
        var text_without_bmp_begin = this.abstractDataController.dataFile.text.Replace("<bmp_begin>", "");
        string[] firstSplit = Regex.Split(text_without_bmp_begin, "<bmp_end>");

        var headerValue = firstSplit[0];
        this.abstractDataController.header = new HeaderData();
        var headerRegex = new Regex("\n");

        var headerParams = headerRegex.Split(headerValue);
        var spriteFileNameHeaderParam = headerParams[ComposerUtil.GetTypeFromAbstract(this.abstractDataController.type)];
        var spriteFileNameValueParam = spriteFileNameHeaderParam.Split(':')[1];
        var spriteFileNameRegex = new Regex("Resources\\\\(.*\\\\)");
        this.abstractDataController.header.sprite_file_name = spriteFileNameRegex.Split(spriteFileNameValueParam.Trim())[2].Replace(".png  w", "");
        this.abstractDataController.header.sprite_folder = abstractDataController.GetHeaderParam(headerParams, CharacterHeaderKeyEnum.SPRITE_FOLDER);

        this.sprites = ComposerUtil.GetSpriteMapper(this.abstractDataController);

        var framesValue = firstSplit[1];
        DataMapperUtil.MapDataToObject(framesValue, out this.frames, this.sprites, this.abstractDataController.header.sprite_file_name);
    }

    private void SetFrameSprite() {
        var centery = Mathf.Abs(selectedFrame.centery / selectedFrame.pic.rect.height - 1);
        var centerx = selectedFrame.centerx / selectedFrame.pic.rect.width;

        var pivot = new Vector2(centerx, centery);
        this.spriteRenderer.sprite = Sprite.Create(selectedFrame.pic.texture, selectedFrame.pic.rect, pivot);
    }

    private void BuildItr() {
        EditorGUILayout.BeginHorizontal();
        this.itrNumber = (ComposerElementsNumberEnum)EditorGUILayout.EnumPopup("itrNumber: ", itrNumber);
        EditorGUILayout.EndHorizontal();
        if (this.itrNumber == ComposerElementsNumberEnum.MAIN) {
            this.loadItr1ByComposer = EditorGUILayout.Toggle("Type of Load Itr dimensions: " +
            "(True = Composer; False = Hitbox + itrNumber)", this.loadItr1ByComposer);
            if (loadItr1ByComposer) {
                this.loadItr1ByComposer = this.abstractDataController.interactionsComposer.ContainsKey(selectedFrame.id);
            }
            this.BuildSpecificItr(this.itrData, this.abstractDataController.interactionsComposer, 1, this.loadItr1ByComposer);
        }

        if (this.itrNumber == ComposerElementsNumberEnum.SECOND) {
            this.loadItr2ByComposer = EditorGUILayout.Toggle("Type of Load Itr dimensions: " +
            "(True = Composer; False = Hitbox + itrNumber)", this.loadItr2ByComposer);
            if (loadItr2ByComposer) {
                this.loadItr2ByComposer = this.abstractDataController.interactionsComposer2.ContainsKey(selectedFrame.id);
            }
            this.BuildSpecificItr(this.itrData2, this.abstractDataController.interactionsComposer2, 2, this.loadItr2ByComposer);
        }

        if (this.itrNumber == ComposerElementsNumberEnum.THIRD) {
            this.loadItr3ByComposer = EditorGUILayout.Toggle("Type of Load Itr dimensions: " +
            "(True = Composer; False = Hitbox + itrNumber)", this.loadItr3ByComposer);
            if (loadItr3ByComposer) {
                this.loadItr3ByComposer = this.abstractDataController.interactionsComposer3.ContainsKey(selectedFrame.id);
            }
            this.BuildSpecificItr(this.itrData3, this.abstractDataController.interactionsComposer3, 3, this.loadItr3ByComposer);
        }

        EditorGUILayout.Separator();
    }

    private void DrawComposerItrs() {
        var transformMain = selectedGameObject.transform.Find("Hitboxes");

        if (transformMain) {
            if (itrNumber != ComposerElementsNumberEnum.MAIN && abstractDataController.interactionsComposer.TryGetValue(selectedFrame.id, out itrData)) {
                var transform1 = transformMain.Find("Hitbox1");

                transform1.localPosition = new Vector3(itrData.x, itrData.y, itrData.z);
                transform1.localScale = new Vector3(itrData.w, itrData.h, itrData.zwidthz);
            } else {
                itrData = new InteractionData();
            }

            if (itrNumber != ComposerElementsNumberEnum.SECOND && abstractDataController.interactionsComposer2.TryGetValue(selectedFrame.id, out itrData2)) {
                var transform2 = transformMain.Find("Hitbox2");

                transform2.localPosition = new Vector3(itrData2.x, itrData2.y, itrData2.z);
                transform2.localScale = new Vector3(itrData2.w, itrData2.h, itrData2.zwidthz);
            } else {
                itrData2 = new InteractionData();
            }

            if (itrNumber != ComposerElementsNumberEnum.THIRD && abstractDataController.interactionsComposer3.TryGetValue(selectedFrame.id, out itrData3)) {
                var transform3 = transformMain.Find("Hitbox3");

                transform3.localPosition = new Vector3(itrData3.x, itrData3.y, itrData3.z);
                transform3.localScale = new Vector3(itrData3.w, itrData3.h, itrData3.zwidthz);
            } else {
                itrData3 = new InteractionData();
            }
        }
    }

    private void BuildSpecificItr(InteractionData specificItrData, Map<int, InteractionData> itrsComposer, int itrNumber, bool loadItrByComposer) {
        var tempGO = new GameObject();
        Transform dimensionsToUse = tempGO.transform;

        if (loadItrByComposer) {
            dimensionsToUse.localPosition = new Vector3(itrsComposer[selectedFrame.id].x, itrsComposer[selectedFrame.id].y, itrsComposer[selectedFrame.id].z);

            var specificHurtbox = selectedGameObject.transform.Find("Opoints").Find("Opoint" + itrNumber);
            specificHurtbox.localPosition = dimensionsToUse.localPosition;
            this.kind = itrsComposer[selectedFrame.id].kind;
            this.action = itrsComposer[selectedFrame.id].action;
            this.dvx = itrsComposer[selectedFrame.id].dvx;
            this.dvy = itrsComposer[selectedFrame.id].dvy;
            this.dvz = itrsComposer[selectedFrame.id].dvz;
        } else {
            dimensionsToUse = selectedGameObject.transform.Find("Opoints").Find("Opoint" + itrNumber);
        }

        if (dimensionsToUse) {
            kind = (ItrKindEnum)EditorGUILayout.EnumPopup("kind: ", kind);
            specificItrData.kind = kind;

            action = EditorGUILayout.IntField("action: ", action);
            specificItrData.action = action;


            EditorGUILayout.BeginHorizontal();
            dvx = EditorGUILayout.FloatField("dvx: ", dvx);
            specificItrData.dvx = dvx;

            dvy = EditorGUILayout.FloatField("dvy: ", dvy);
            specificItrData.dvy = dvy;
            EditorGUILayout.EndHorizontal();

            dvz = EditorGUILayout.FloatField("dvz: ", dvz);
            specificItrData.dvz = dvz;

            specificItrData.x = EditorGUILayout.FloatField("x: ", dimensionsToUse.localPosition.x);
            specificItrData.y = EditorGUILayout.FloatField("y: ", dimensionsToUse.localPosition.y);
            specificItrData.z = EditorGUILayout.FloatField("z: ", dimensionsToUse.localPosition.z);
            DestroyImmediate(tempGO);

            EditorGUILayout.Separator();

            if (GUILayout.Button("Save OPOINT " + itrNumber)) {
                if (itrsComposer.ContainsKey(selectedFrame.id)) {
                    itrsComposer.Remove(selectedFrame.id);
                }
                specificItrData.itrNumber = itrNumber;
                itrsComposer.Add(selectedFrame.id, specificItrData);
                Debug.Log("Save Done!");
            }

            if (GUILayout.Button("Remove OPOINT " + itrNumber)) {
                itrsComposer.Remove(selectedFrame.id);
                Debug.Log("Remove Done!");
            }
        } else {
            throw new NullReferenceException("Selected Game Object must have hitboxes with itr number. (Hitbox1, Hitbox2, Hitbox3)");
        }
    }
}
