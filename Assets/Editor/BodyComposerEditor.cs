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

    public BodyKindEnum kind;
    public bool wallCheck;

    private int copyIdRangeBegin;
    private int copyIdRangeEnd;

    private int idRangeBegin;
    private int idRangeEnd;
    private int idSelectionSearch;
    private int frameIdSelected;

    private BodyData bodyData;
    private bool loadBdy1ByComposer;

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
                this.DrawComposerBodies();
                this.BuildBdy();
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

        switch (this.abstractDataController.type) {
            case ObjectTypeEnum.CHARACTER:
                this.abstractDataController.header.sprite_folder = abstractDataController.GetHeaderParam(headerParams, CharacterHeaderKeyEnum.SPRITE_FOLDER);
                break;
            case ObjectTypeEnum.POWER:
                this.abstractDataController.header.sprite_folder = abstractDataController.GetHeaderParam(headerParams, PowerHeaderKeyEnum.SPRITE_FOLDER);
                break;
            default:
                throw new MissingFieldException("Object type not found or not implemented!");
        }

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
        this.loadBdy1ByComposer = EditorGUILayout.Toggle("Type of Load Bdy dimensions: " +
        "(True = Composer; False = Hurtbox1)", this.loadBdy1ByComposer);
        if (loadBdy1ByComposer) {
            this.loadBdy1ByComposer = this.abstractDataController.bodysComposer.ContainsKey(selectedFrame.id);
        }
        this.BuildSpecificBdy(this.bodyData, this.abstractDataController.bodysComposer, 1, this.loadBdy1ByComposer);

        EditorGUILayout.Separator();
    }

    private void DrawComposerBodies() {
        var transformMain = selectedGameObject.transform.Find("Hurtbox");

        if (transformMain) {
            if (abstractDataController.bodysComposer.TryGetValue(selectedFrame.id, out bodyData)) {
                transformMain.localPosition = new Vector3(bodyData.x, bodyData.y, bodyData.z);
                transformMain.localScale = new Vector3(bodyData.w, bodyData.h, bodyData.zwidth);
            } else {
                bodyData = new BodyData();
            }
        }
    }

    private void BuildSpecificBdy(BodyData specificBodyData, Map<int, BodyData> bodysComposer, int bodyNumber, bool loadBdyByComposer) {
        var tempGO = new GameObject();
        Transform dimensionsToUse = tempGO.transform;

        if (loadBdyByComposer) {
            dimensionsToUse.localPosition = new Vector3(bodysComposer[selectedFrame.id].x, bodysComposer[selectedFrame.id].y, bodysComposer[selectedFrame.id].z);
            dimensionsToUse.localScale = new Vector3(bodysComposer[selectedFrame.id].w, bodysComposer[selectedFrame.id].h, bodysComposer[selectedFrame.id].zwidth);

            var specificHurtbox = selectedGameObject.transform.Find("Hurtbox");
            specificHurtbox.localPosition = dimensionsToUse.localPosition;
            specificHurtbox.localScale = dimensionsToUse.localScale;
            this.kind = bodysComposer[selectedFrame.id].kind;
            this.wallCheck = bodysComposer[selectedFrame.id].wallCheck;
        } else {
            dimensionsToUse = selectedGameObject.transform.Find("Hurtbox");
        }

        if (dimensionsToUse) {
            this.kind = (BodyKindEnum)EditorGUILayout.EnumPopup("kind: ", kind);
            specificBodyData.kind = kind;

            specificBodyData.x = EditorGUILayout.FloatField("x: ", dimensionsToUse.localPosition.x);
            specificBodyData.y = EditorGUILayout.FloatField("y: ", dimensionsToUse.localPosition.y);
            specificBodyData.z = EditorGUILayout.FloatField("z: ", dimensionsToUse.localPosition.z);

            specificBodyData.w = EditorGUILayout.FloatField("w: ", dimensionsToUse.localScale.x);
            specificBodyData.h = EditorGUILayout.FloatField("h: ", dimensionsToUse.localScale.y);

            specificBodyData.zwidth = EditorGUILayout.FloatField("zwidth: ", dimensionsToUse.localScale.z);
            this.wallCheck = EditorGUILayout.Toggle("wallCheck: ", wallCheck);
            specificBodyData.wallCheck = wallCheck;

            DestroyImmediate(tempGO);

            EditorGUILayout.Separator();

            if (GUILayout.Button("Save BDY " + bodyNumber)) {
                if (bodysComposer.ContainsKey(selectedFrame.id)) {
                    bodysComposer.Remove(selectedFrame.id);
                }
                specificBodyData.hasValue = true;
                bodysComposer.Add(selectedFrame.id, specificBodyData);
                Debug.Log("Save Done!");
            }

            if (GUILayout.Button("Remove BDY " + bodyNumber)) {
                bodysComposer.Remove(selectedFrame.id);
                Debug.Log("Remove Done!");
            }

            EditorGUILayout.Separator();

            idRangeBegin = EditorGUILayout.IntField("Range Begin: ", idRangeBegin);
            idRangeEnd = EditorGUILayout.IntField("End Begin: ", idRangeEnd);
            if (GUILayout.Button("Save BDY " + bodyNumber + " in range ids")) {
                for (int id = idRangeBegin; id <= idRangeEnd; id++) {
                    if (bodysComposer.ContainsKey(id)) {
                        bodysComposer.Remove(id);
                    }
                    specificBodyData.hasValue = true;
                    bodysComposer.Add(id, specificBodyData);
                }
                Debug.Log("Save Done!");
            }
            if (GUILayout.Button("Remove BDY " + bodyNumber + " in range ids")) {
                for (int id = idRangeBegin; id <= idRangeEnd; id++) {
                    bodysComposer.Remove(id);
                }
                Debug.Log("Remove Done!");
            }

            copyIdRangeBegin = EditorGUILayout.IntField("Copy Range Begin: ", copyIdRangeBegin);
            copyIdRangeEnd = EditorGUILayout.IntField("Copy End Begin: ", copyIdRangeEnd);
            if (GUILayout.Button("Copy BDY " + bodyNumber + " in range ids")) {
                if ((idRangeEnd - idRangeBegin) - (copyIdRangeEnd - copyIdRangeBegin) == 0) {
                    int copyId = copyIdRangeBegin;
                    for (int id = idRangeBegin; id <= idRangeEnd; id++) {
                        if (bodysComposer.ContainsKey(id)) {
                            bodysComposer.Remove(id);
                        }

                        if (bodysComposer.ContainsKey(copyId)) {
                            bodysComposer.Add(id, bodysComposer[copyId]);
                            copyId++;
                        } else {
                            throw new NullReferenceException("Copy Frame not found in BodyComposer to persist!");
                        }
                    }
                    Debug.Log("Save Done!");
                } else {
                    Debug.Log("Quantity of copy frames and save frames are different!");
                }
            }

            EditorGUILayout.Separator();
        } else {
            throw new NullReferenceException("Selected Game Object must have hurtboxes with body number. (Hurtbox1, Hurtbox2, Hurtbox3)");
        }
    }
}
