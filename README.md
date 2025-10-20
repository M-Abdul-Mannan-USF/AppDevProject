# 📰 News Filterer and Bias Checker

## 📖 Overview  
This project is a **static website prototype** built as part of the *ISM 6225 – Application Development for Analytics* course.  
The website represents the design and structure of the final project, focusing on identifying and visualizing **bias in news articles**. It includes multiple interconnected pages, CRUD navigation, data visualizations, and a chatbot powered by **Botpress**.

---

## 🌐 Domain  
**Domain:** News Filterer and Bias Checker  
The project aims to analyze and visualize potential bias in news articles by examining word usage, sentiment, and publisher tendencies.

---

## 📊 Dataset Used  
**Dataset:** [News Dataset for News Bias Analysis – Kaggle](https://www.kaggle.com/datasets/articoder/news-dataset-for-news-bias-analysis)  
This dataset contains thousands of news articles with information such as:
- Headline and full article text  
- Source and publication date  
- Category and topic tags  
- Bias or sentiment indicators  

The dataset is used to simulate how our system can detect and visualize bias patterns across different news sources.

---

## 🧠 Features

### 1. Home Page
- Introduces the project and its purpose.  
- Provides navigation links to all other pages.  

### 2. Data Visualization Page
- Displays three **interactive charts** created using Chart.js:  
  - **Bar Chart** – distribution of news sources by bias  
  - **Pie Chart** – sentiment breakdown of articles  
  - **Line Chart** – trend of biased vs unbiased news over time  
- Uses meaningful dummy data related to news and bias.

### 3. About Us Page
- Lists team members with their **roles and contributions**.  
- Displays the **logical data model** (with two one-to-many relationships).  
- Provides a link to the project’s **GitHub repository**.  

### 4. CRUD Operation Pages
- **Create:** Explains how new articles could be added in the dynamic version.  
- **Read:** Displays dummy news entries.  
- **Update:** Demonstrates simulated data modification.  
- **Delete:** Shows how records could be removed.  

### 5. Botpress Chatbot
- Embedded chatbot that answers user questions about the project and dataset.  
- Built using Botpress and integrated directly within the website.  

---

## 🧩 Logical Data Model  
The logical model includes two one-to-many relationships:  
- **Sources → Articles** (one source publishes many articles)  
- **Articles → Comments** (one article can have many comments)  

An image of the model is displayed on the *About Us* page.

---

## 📈 Technologies Used  
- **HTML5, CSS3, JavaScript (ES6)** – for structure and interactivity  
- **Chart.js** – for interactive data visualization  
- **Bootstrap 5** – for responsive design  
- **Botpress** – for chatbot integration  
- **GitHub** – for version control and collaboration  

---

## 🧰 Future Enhancements (Final Project Plan)
- Integrate a **news bias detection API** or NLP-based service.  
- Implement **live CRUD operations** with a backend database (e.g., MongoDB or Firebase).  
- Display **real-time bias metrics** in the Chart.js graphs.  
- Enhance chatbot capabilities to analyze specific news headlines dynamically.

---

## 🤝 Team Contributions  
Each team member contributed to different aspects of the project:
- **Frontend Design:** HTML, CSS, and responsive layout  
- **Visualization:** Chart.js integration and dummy data creation  
- **Bot Integration:** Embedding and configuring Botpress chatbot  
- **Documentation:** README, self-reflection, and logical model  

---

## 🔗 Useful Links  
- **Dataset:** [News Dataset for News Bias Analysis](https://www.kaggle.com/datasets/articoder/news-dataset-for-news-bias-analysis)  
