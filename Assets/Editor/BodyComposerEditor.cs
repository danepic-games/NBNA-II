using System;
using System.Linq;
using System.Text.RegularExpressions;
using SerializableHelper;
using UnityEditor;
using UnityEngine;

public class BodyComposerEditor : EditorWindow {

    private GameObject selectedGameObject;

    private SpriteRenderer spriteRenderer;
    private AbstractDataController abstractDataController;
    private AbstractDataController activeAbstractDataController;

    private Vector2 scrollPos;
    private int idSelection;
    private string idSelectionSearch;
    private int frameIdSelected;

    private BodyData bodyData;
    private BodyData bodyData2;
    private BodyData bodyData3;
    private bool openBdy;
    private bool openBdy2;
    private bool openBdy3;
    private bool loadBdy1ByComposer;
    private bool loadBdy2ByComposer;
    private bool loadBdy3ByComposer;

    private Map<int, Sprite> sprites;
    private Map<int, FrameData> frames;
    private FrameData selectedFrame;


    [MenuItem("Frame/Body Composer")]
    public static void Init() {
        var window = GetWindow<BodyComposerEditor>("Body Composer");
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
                    this.DrawComposerBodies();
                    this.BuildBdy();
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

    private void BuildBdy() {
        EditorGUILayout.BeginHorizontal();
        this.openBdy = EditorGUILayout.Toggle("Show BDY 1", this.openBdy);
        EditorGUILayout.EndHorizontal();
        if (this.openBdy) {
            this.loadBdy1ByComposer = EditorGUILayout.Toggle("Type of Load Bdy dimensions: " +
            "(True = Composer; False = Hurtbox1)", this.loadBdy1ByComposer);
            if (loadBdy1ByComposer) {
                this.loadBdy1ByComposer = this.abstractDataController.bodysComposer.ContainsKey(selectedFrame.id);
            }
            this.BuildSpecificBdy(this.bodyData, this.abstractDataController.bodysComposer, 1, this.loadBdy1ByComposer);
        }

        EditorGUILayout.BeginHorizontal();
        this.openBdy2 = EditorGUILayout.Toggle("Show BDY 2", this.openBdy2);
        EditorGUILayout.EndHorizontal();
        if (this.openBdy2) {
            this.loadBdy2ByComposer = EditorGUILayout.Toggle("Type of Load Bdy dimensions: " +
            "(True = Composer; False = Hurtbox2)", this.loadBdy2ByComposer);
            if (loadBdy2ByComposer) {
                this.loadBdy2ByComposer = this.abstractDataController.bodysComposer2.ContainsKey(selectedFrame.id);
            }
            this.BuildSpecificBdy(this.bodyData2, this.abstractDataController.bodysComposer2, 2, this.loadBdy2ByComposer);
        }

        EditorGUILayout.BeginHorizontal();
        this.openBdy3 = EditorGUILayout.Toggle("Show BDY 3", this.openBdy3);
        EditorGUILayout.EndHorizontal();
        if (this.openBdy3) {
            this.loadBdy3ByComposer = EditorGUILayout.Toggle("Type of Load Bdy dimensions: " +
            "(True = Composer; False = Hurtbox3)", this.loadBdy3ByComposer);
            if (loadBdy3ByComposer) {
                this.loadBdy3ByComposer = this.abstractDataController.bodysComposer3.ContainsKey(selectedFrame.id);
            }
            this.BuildSpecificBdy(this.bodyData3, this.abstractDataController.bodysComposer3, 3, this.loadBdy3ByComposer);
        }

        EditorGUILayout.Separator();
    }

    private void DrawComposerBodies() {
        var transformMain = selectedGameObject.transform.Find("Hurtboxes");

        if (transformMain) {
            if (!openBdy && abstractDataController.bodysComposer.TryGetValue(selectedFrame.id, out bodyData)) {
                var transform1 = transformMain.Find("Hurtbox1");

                transform1.localPosition = new Vector3(bodyData.x, bodyData.y, bodyData.z);
                transform1.localScale = new Vector3(bodyData.w, bodyData.h, bodyData.zwidth);
            } else {
                bodyData = new BodyData();
            }

            if (!openBdy2 && abstractDataController.bodysComposer2.TryGetValue(selectedFrame.id, out bodyData2)) {
                var transform2 = transformMain.Find("Hurtbox2");

                transform2.localPosition = new Vector3(bodyData2.x, bodyData2.y, bodyData2.z);
                transform2.localScale = new Vector3(bodyData2.w, bodyData2.h, bodyData2.zwidth);
            } else {
                bodyData2 = new BodyData();
            }

            if (!openBdy3 && abstractDataController.bodysComposer3.TryGetValue(selectedFrame.id, out bodyData3)) {
                var transform3 = transformMain.Find("Hurtbox3");

                transform3.localPosition = new Vector3(bodyData3.x, bodyData3.y, bodyData3.z);
                transform3.localScale = new Vector3(bodyData3.w, bodyData3.h, bodyData3.zwidth);
            } else {
                bodyData3 = new BodyData();
            }
        }
    }

    private void BuildSpecificBdy(BodyData specificBodyData, Map<int, BodyData> bodysComposer, int bodyNumber, bool loadBdyByComposer) {
        var tempGO = new GameObject();
        Transform dimensionsToUse = tempGO.transform;

        if (loadBdyByComposer) {
            dimensionsToUse.localPosition = new Vector3(bodysComposer[selectedFrame.id].x, bodysComposer[selectedFrame.id].y, bodysComposer[selectedFrame.id].z);
            dimensionsToUse.localScale = new Vector3(bodysComposer[selectedFrame.id].w, bodysComposer[selectedFrame.id].h, bodysComposer[selectedFrame.id].zwidth);

            var specificHurtbox = selectedGameObject.transform.Find("Hurtboxes").Find("Hurtbox" + bodyNumber);
            specificHurtbox.localPosition = dimensionsToUse.localPosition;
            specificHurtbox.localScale = dimensionsToUse.localScale;
        } else {
            dimensionsToUse = selectedGameObject.transform.Find("Hurtboxes").Find("Hurtbox" + bodyNumber);
        }

        if (dimensionsToUse) {
            specificBodyData.kind = (BodyKindEnum)EditorGUILayout.EnumPopup("kind: ", specificBodyData.kind);

            specificBodyData.x = EditorGUILayout.FloatField("x: ", dimensionsToUse.localPosition.x);
            specificBodyData.y = EditorGUILayout.FloatField("y: ", dimensionsToUse.localPosition.y);
            specificBodyData.z = EditorGUILayout.FloatField("z: ", dimensionsToUse.localPosition.z);

            specificBodyData.w = EditorGUILayout.FloatField("w: ", dimensionsToUse.localScale.x);
            specificBodyData.h = EditorGUILayout.FloatField("h: ", dimensionsToUse.localScale.y);
            specificBodyData.zwidth = EditorGUILayout.FloatField("zwidth: ", dimensionsToUse.localScale.z);
            DestroyImmediate(tempGO);

            EditorGUILayout.Separator();

            if (GUILayout.Button("Save BDY " + bodyNumber)) {
                if (bodysComposer.ContainsKey(selectedFrame.id)) {
                    bodysComposer.Remove(selectedFrame.id);
                }
                specificBodyData.bodyNumber = bodyNumber;
                bodysComposer.Add(selectedFrame.id, specificBodyData);
            }

            if (GUILayout.Button("Remove BDY " + bodyNumber)) {
                bodysComposer.Remove(selectedFrame.id);
            }
        } else {
            throw new NullReferenceException("Selected Game Object must have hurtboxes with body number. (Hurtbox1, Hurtbox2, Hurtbox3)");
        }
    }
}
