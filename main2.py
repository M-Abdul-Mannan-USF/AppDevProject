# from fastapi import FastAPI, Request
# from transformers import AutoTokenizer, AutoModelForSequenceClassification
# import torch

# app = FastAPI()

# # Load model and tokenizer
# MODEL_NAME = "himel7/bias-detector"
# tokenizer = AutoTokenizer.from_pretrained(MODEL_NAME)
# model = AutoModelForSequenceClassification.from_pretrained(MODEL_NAME)

# @app.post("/predict")
# async def predict(request: Request):
#     data = await request.json()
#     text = data.get("text", "")

#     # Tokenize
#     inputs = tokenizer(text, return_tensors="pt", truncation=True, padding=True, max_length=512)

#     # Predict
#     with torch.no_grad():
#         outputs = model(**inputs)
#         logits = outputs.logits
#         predicted_class = torch.argmax(logits, dim=1).item()

#     # Labels based on model config
#     labels = model.config.id2label
#     return {"label": labels[predicted_class]}

from fastapi import FastAPI, Request
from transformers import AutoTokenizer, AutoModelForSequenceClassification
import torch

app = FastAPI()

# Load model and tokenizer
MODEL_NAME = "matous-volf/political-leaning-politics"
TOKENIZER_NAME = "launch/POLITICS"     # explicit tokenizer repo

tokenizer = AutoTokenizer.from_pretrained(TOKENIZER_NAME)
# tokenizer = AutoTokenizer.from_pretrained(MODEL_NAME)
model = AutoModelForSequenceClassification.from_pretrained(MODEL_NAME)

@app.post("/predict")
async def predict(request: Request):
    data = await request.json()
    text = data.get("text", "")

    # Tokenize
    inputs = tokenizer(text, return_tensors="pt", truncation=True, padding=True, max_length=512)

    # Predict
    with torch.no_grad():
        outputs = model(**inputs)
        logits = outputs.logits
        predicted_class = torch.argmax(logits, dim=1).item()

    # Labels based on model config
    labels = model.config.id2label  # e.g. {0: "Left", 1: "Center", 2: "Right"}
    if labels[predicted_class] == "LABEL_0": 
        output = "Left"
    elif labels[predicted_class] == "LABEL_1": 
        output = "Center"
    else:
        output = "Right"
    return {"label": output}
