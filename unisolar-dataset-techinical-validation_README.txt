### README for unisolar-dataset-techinical-validation.ipynb

#### Purpose:
This notebook performs technical validation on the dataset using Python and Pandas.

#### Markdown Overview:
# UNISOLAR Dataset Technical Validation

This notebook presents a technical validation for selected subset of data in the UNISOLAR dataset
### Load the needed libraries

#### Sample Code:
```python
# python
import os
import warnings

# data science
import pandas as pd
import matplotlib.pyplot as plt

%matplotlib inline
plt.rcParams['figure.figsize'] = [18, 5]
warnings.filterwarnings('ignore')
warnings.simplefilter('ignore')
BASE_PATH = "../input/unisolar/unisolar"
```
