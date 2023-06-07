using System;
using System.Linq;
using System.Text.RegularExpressions;
using SerializableHelper;
using UnityEditor;
using UnityEngine;

public class OpointComposerEditor : EditorWindow {

    private GameObject selectedGameObject;

    private SpriteRenderer spriteRenderer;
    private AbstractDataController abstractDataController;
    private AbstractDataController activeAbstractDataController;

    public ObjectPointKindEnum kind;
    public int action;
    public float dvx;
    public float dvy;
    public float dvz;
    public string object_id;
    public bool facing;
    public int quantity;
    public float z_division_per_quantity;

    private int idSelectionSearch;
    private int frameIdSelected;

    private ObjectPointData opointData;
    private ObjectPointData opointData2;
    private ObjectPointData opointData3;
    private ComposerElementsNumberEnum opointNumber;
    private bool loadOpoint1ByComposer;
    private bool loadOpoint2ByComposer;
    private bool loadOpoint3ByComposer;

    private Map<int, Sprite> sprites;
    private Map<int, FrameData> frames;
    private FrameData selectedFrame;


    [MenuItem("Frame/Opoint Composer")]
    public static void Init() {
        var window = GetWindow<OpointComposerEditor>("Opoint Composer");
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
                this.DrawComposerOpoints();
                this.BuildOpoint();
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

    private void BuildOpoint() {
        EditorGUILayout.BeginHorizontal();
        this.opointNumber = (ComposerElementsNumberEnum)EditorGUILayout.EnumPopup("opointNumber: ", opointNumber);
        EditorGUILayout.EndHorizontal();
        if (this.opointNumber == ComposerElementsNumberEnum.MAIN) {
            this.loadOpoint1ByComposer = EditorGUILayout.Toggle("Type of Load Opoint dimensions: " +
            "(True = Composer; False = Opoints + opointNumber)", this.loadOpoint1ByComposer);
            if (loadOpoint1ByComposer) {
                this.loadOpoint1ByComposer = this.abstractDataController.opointsComposer.ContainsKey(selectedFrame.id);
            }
            this.BuildSpecificItr(this.opointData, this.abstractDataController.opointsComposer, 1, this.loadOpoint1ByComposer);
        }

        if (this.opointNumber == ComposerElementsNumberEnum.SECOND) {
            this.loadOpoint2ByComposer = EditorGUILayout.Toggle("Type of Load Opoint dimensions: " +
            "(True = Composer; False = Opoints + opointNumber)", this.loadOpoint2ByComposer);
            if (loadOpoint2ByComposer) {
                this.loadOpoint2ByComposer = this.abstractDataController.opointsComposer2.ContainsKey(selectedFrame.id);
            }
            this.BuildSpecificItr(this.opointData2, this.abstractDataController.opointsComposer2, 2, this.loadOpoint2ByComposer);
        }

        if (this.opointNumber == ComposerElementsNumberEnum.THIRD) {
            this.loadOpoint3ByComposer = EditorGUILayout.Toggle("Type of Load Opoint dimensions: " +
            "(True = Composer; False = Opoints + opointNumber)", this.loadOpoint3ByComposer);
            if (loadOpoint3ByComposer) {
                this.loadOpoint3ByComposer = this.abstractDataController.opointsComposer3.ContainsKey(selectedFrame.id);
            }
            this.BuildSpecificItr(this.opointData3, this.abstractDataController.opointsComposer3, 3, this.loadOpoint3ByComposer);
        }

        EditorGUILayout.Separator();
    }

    private void DrawComposerOpoints() {
        var transformMain = selectedGameObject.transform.Find("Opoints");

        if (transformMain) {
            if (opointNumber != ComposerElementsNumberEnum.MAIN && abstractDataController.opointsComposer.TryGetValue(selectedFrame.id, out opointData)) {
                var transform1 = transformMain.Find("Opoint1");

                transform1.localPosition = new Vector3(opointData.x, opointData.y, opointData.z);
            } else {
                opointData = new ObjectPointData();
            }

            if (opointNumber != ComposerElementsNumberEnum.SECOND && abstractDataController.opointsComposer2.TryGetValue(selectedFrame.id, out opointData2)) {
                var transform2 = transformMain.Find("Opoint2");

                transform2.localPosition = new Vector3(opointData2.x, opointData2.y, opointData2.z);
            } else {
                opointData2 = new ObjectPointData();
            }

            if (opointNumber != ComposerElementsNumberEnum.THIRD && abstractDataController.opointsComposer3.TryGetValue(selectedFrame.id, out opointData3)) {
                var transform3 = transformMain.Find("Opoint3");

                transform3.localPosition = new Vector3(opointData3.x, opointData3.y, opointData3.z);
            } else {
                opointData3 = new ObjectPointData();
            }
        }
    }

    private void BuildSpecificItr(ObjectPointData specificObjectPointData, Map<int, ObjectPointData> opointsComposer, int opointNumber, bool loadOpointByComposer) {
        var tempGO = new GameObject();
        Transform dimensionsToUse = tempGO.transform;

        if (loadOpointByComposer) {
            dimensionsToUse.localPosition = new Vector3(opointsComposer[selectedFrame.id].x, opointsComposer[selectedFrame.id].y, opointsComposer[selectedFrame.id].z);

            var specificHurtbox = selectedGameObject.transform.Find("Opoints").Find("Opoint" + opointNumber);
            specificHurtbox.localPosition = dimensionsToUse.localPosition;
            this.kind = opointsComposer[selectedFrame.id].kind;
            this.action = opointsComposer[selectedFrame.id].action;
            this.dvx = opointsComposer[selectedFrame.id].dvx;
            this.dvy = opointsComposer[selectedFrame.id].dvy;
            this.dvz = opointsComposer[selectedFrame.id].dvz;
            this.object_id = opointsComposer[selectedFrame.id].object_id;
            this.facing = opointsComposer[selectedFrame.id].facing;
            this.quantity = opointsComposer[selectedFrame.id].quantity;
            this.z_division_per_quantity = opointsComposer[selectedFrame.id].z_division_per_quantity;
        } else {
            dimensionsToUse = selectedGameObject.transform.Find("Opoints").Find("Opoint" + opointNumber);
        }

        if (dimensionsToUse) {
            kind = (ObjectPointKindEnum)EditorGUILayout.EnumPopup("kind: ", kind);
            specificObjectPointData.kind = kind;

            action = EditorGUILayout.IntField("action: ", action);
            specificObjectPointData.action = action;


            EditorGUILayout.BeginHorizontal();
            dvx = EditorGUILayout.FloatField("dvx: ", dvx);
            specificObjectPointData.dvx = dvx;

            dvy = EditorGUILayout.FloatField("dvy: ", dvy);
            specificObjectPointData.dvy = dvy;
            EditorGUILayout.EndHorizontal();

            dvz = EditorGUILayout.FloatField("dvz: ", dvz);
            specificObjectPointData.dvz = dvz;

            object_id = EditorGUILayout.TextField("object_id: ", object_id);
            specificObjectPointData.object_id = object_id;

            EditorGUILayout.BeginHorizontal();
            facing = EditorGUILayout.Toggle("facing: ", facing);
            specificObjectPointData.facing = facing;

            quantity = EditorGUILayout.IntField("quantity: ", quantity);
            specificObjectPointData.quantity = quantity;
            EditorGUILayout.EndHorizontal();

            z_division_per_quantity = EditorGUILayout.FloatField("z_division_per_quantity: ", z_division_per_quantity);
            specificObjectPointData.z_division_per_quantity = z_division_per_quantity;

            specificObjectPointData.x = EditorGUILayout.FloatField("x: ", dimensionsToUse.localPosition.x);
            specificObjectPointData.y = EditorGUILayout.FloatField("y: ", dimensionsToUse.localPosition.y);
            specificObjectPointData.z = EditorGUILayout.FloatField("z: ", dimensionsToUse.localPosition.z);
            DestroyImmediate(tempGO);

            EditorGUILayout.Separator();

            if (GUILayout.Button("Save OPOINT " + opointNumber)) {
                if (opointsComposer.ContainsKey(selectedFrame.id)) {
                    opointsComposer.Remove(selectedFrame.id);
                }
                specificObjectPointData.opointNumber = opointNumber;
                opointsComposer.Add(selectedFrame.id, specificObjectPointData);
                Debug.Log("Save Done!");
            }

            if (GUILayout.Button("Remove OPOINT " + opointNumber)) {
                opointsComposer.Remove(selectedFrame.id);
                Debug.Log("Remove Done!");
            }
        } else {
            throw new NullReferenceException("Selected Game Object must have opoints with itr number. (Opoint1, Opoint2, Opoint3)");
        }
    }
}
