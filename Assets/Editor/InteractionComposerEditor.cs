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

    private Vector2 scrollPos;
    private int idSelection;
    private string idSelectionSearch;
    private int frameIdSelected;

    private InteractionData itrData;
    private InteractionData itrData2;
    private InteractionData itrData3;
    private bool openItr;
    private bool openItr2;
    private bool openItr3;
    private bool loadItr1ByComposer;
    private bool loadItr2ByComposer;
    private bool loadItr3ByComposer;

    private Map<int, Sprite> sprites;
    private Map<int, FrameData> frames;
    private FrameData selectedFrame;


    [MenuItem("Frame/Itr Composer")]
    public static void Init() {
        var window = GetWindow<InteractionComposerEditor>("Itr Composer");
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
            idSelectionSearch = EditorGUILayout.TextField("Search Frame", idSelectionSearch);
            EditorGUILayout.EndHorizontal();

            if (!string.IsNullOrEmpty(idSelectionSearch) && idSelectionSearch.All(char.IsDigit)) {
                var frameIds = this.frames.Where(frame => frame.Key.ToString()
                .Equals(idSelectionSearch)).Select(frame => frame.Key.ToString()).ToArray();

                if (frameIds.Length == 1) {
                    frameIdSelected = int.Parse(frameIds[0]);
                    selectedFrame = this.frames[frameIdSelected];
                    this.SetFrameSprite();
                    this.DrawComposerItrs();
                    this.BuildItr();
                }
            } else {
                idSelectionSearch = "";
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
        this.openItr = EditorGUILayout.Toggle("Show ITR 1", this.openItr);
        EditorGUILayout.EndHorizontal();
        if (this.openItr) {
            this.loadItr1ByComposer = EditorGUILayout.Toggle("Type of Load Itr dimensions: " +
            "(True = Composer; False = Hitbox1)", this.loadItr1ByComposer);
            if (loadItr1ByComposer) {
                this.loadItr1ByComposer = this.abstractDataController.interactionsComposer.ContainsKey(selectedFrame.id);
            }
            this.BuildSpecificItr(this.itrData, this.abstractDataController.interactionsComposer, 1, this.loadItr1ByComposer);
        }

        EditorGUILayout.BeginHorizontal();
        this.openItr2 = EditorGUILayout.Toggle("Show ITR 2", this.openItr2);
        EditorGUILayout.EndHorizontal();
        if (this.openItr2) {
            this.loadItr2ByComposer = EditorGUILayout.Toggle("Type of Load Itr dimensions: " +
            "(True = Composer; False = Hitbox2)", this.loadItr2ByComposer);
            if (loadItr2ByComposer) {
                this.loadItr2ByComposer = this.abstractDataController.interactionsComposer2.ContainsKey(selectedFrame.id);
            }
            this.BuildSpecificItr(this.itrData2, this.abstractDataController.interactionsComposer2, 2, this.loadItr2ByComposer);
        }

        EditorGUILayout.BeginHorizontal();
        this.openItr3 = EditorGUILayout.Toggle("Show ITR 3", this.openItr3);
        EditorGUILayout.EndHorizontal();
        if (this.openItr3) {
            this.loadItr3ByComposer = EditorGUILayout.Toggle("Type of Load Itr dimensions: " +
            "(True = Composer; False = Hitbox3)", this.loadItr3ByComposer);
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
            if (!openItr && abstractDataController.interactionsComposer.TryGetValue(selectedFrame.id, out itrData)) {
                var transform1 = transformMain.Find("Hitbox1");

                transform1.localPosition = new Vector3(itrData.x, itrData.y, itrData.z);
                transform1.localScale = new Vector3(itrData.w, itrData.h, itrData.zwidthz);
            } else {
                itrData = new InteractionData();
            }

            if (!openItr2 && abstractDataController.interactionsComposer2.TryGetValue(selectedFrame.id, out itrData2)) {
                var transform2 = transformMain.Find("Hitbox2");

                transform2.localPosition = new Vector3(itrData2.x, itrData2.y, itrData2.z);
                transform2.localScale = new Vector3(itrData2.w, itrData2.h, itrData2.zwidthz);
            } else {
                itrData2 = new InteractionData();
            }

            if (!openItr3 && abstractDataController.interactionsComposer3.TryGetValue(selectedFrame.id, out itrData3)) {
                var transform3 = transformMain.Find("Hitbox3");

                transform3.localPosition = new Vector3(itrData3.x, itrData3.y, itrData3.z);
                transform3.localScale = new Vector3(itrData3.w, itrData3.h, itrData3.zwidthz);
            } else {
                itrData3 = new InteractionData();
            }
        }
    }

    private void BuildSpecificItr(InteractionData specificInteractionData, Map<int, InteractionData> itrsComposer, int itrNumber, bool loadItrByComposer) {
        var tempGO = new GameObject();
        Transform dimensionsToUse = tempGO.transform;

        if (loadItrByComposer) {
            dimensionsToUse.localPosition = new Vector3(itrsComposer[selectedFrame.id].x, itrsComposer[selectedFrame.id].y, itrsComposer[selectedFrame.id].z);
            dimensionsToUse.localScale = new Vector3(itrsComposer[selectedFrame.id].w, itrsComposer[selectedFrame.id].h, itrsComposer[selectedFrame.id].zwidthz);

            var specificHurtbox = selectedGameObject.transform.Find("Hitboxes").Find("Hitbox" + itrNumber);
            specificHurtbox.localPosition = dimensionsToUse.localPosition;
            specificHurtbox.localScale = dimensionsToUse.localScale;
        } else {
            dimensionsToUse = selectedGameObject.transform.Find("Hitboxes").Find("Hitbox" + itrNumber);
        }

        if (dimensionsToUse) {
            specificInteractionData.kind = (ItrKindEnum)EditorGUILayout.EnumPopup("kind: ", specificInteractionData.kind);

            specificInteractionData.x = EditorGUILayout.FloatField("x: ", dimensionsToUse.localPosition.x);
            specificInteractionData.y = EditorGUILayout.FloatField("y: ", dimensionsToUse.localPosition.y);
            specificInteractionData.z = EditorGUILayout.FloatField("z: ", dimensionsToUse.localPosition.z);

            specificInteractionData.w = EditorGUILayout.FloatField("w: ", dimensionsToUse.localScale.x);
            specificInteractionData.h = EditorGUILayout.FloatField("h: ", dimensionsToUse.localScale.y);
            specificInteractionData.zwidthz = EditorGUILayout.FloatField("zwidth: ", dimensionsToUse.localScale.z);
            DestroyImmediate(tempGO);

            EditorGUILayout.Separator();

            if (GUILayout.Button("Save ITR " + itrNumber)) {
                if (itrsComposer.ContainsKey(selectedFrame.id)) {
                    itrsComposer.Remove(selectedFrame.id);
                }
                specificInteractionData.itrNumber = itrNumber;
                itrsComposer.Add(selectedFrame.id, specificInteractionData);
            }

            if (GUILayout.Button("Remove ITR " + itrNumber)) {
                itrsComposer.Remove(selectedFrame.id);
            }
        } else {
            throw new NullReferenceException("Selected Game Object must have hurtboxes with itr number. (Hitbox1, Hitbox2, Hitbox3)");
        }
    }
}
