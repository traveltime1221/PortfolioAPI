## py 指令
若使用 pyenv 管理版本，最好每個專案搭配虛擬環境（如 venv 或 pyenv-virtualenv）來避免版本衝突。

### pyenv: Python 版本管理工具（用來在一台機器上安裝並切換多個 Python 版本。）
#### 如果要更新 pyenv 可使用下面指令
使用 homebrew 指令安裝
brew update # 保持最新版本

##### 如果沒有安裝 pyenv
brew install pyenv
設定 shell 環境

nano ~/.zshrc
```
export PYENV_ROOT="$(brew --prefix)/opt/pyenv"
export PATH="$PYENV_ROOT/bin:$PATH"
eval "$(pyenv init --path)"
eval "$(pyenv init -)"
```
存檔： Control 鍵和字母 O
確認： Enter 確認
退出： Ctrl + X 退出 nano 編輯器
執行： source ~/.zshrc
確認： pyenv --version


##### 如果有安裝 pyenv
brew upgrade pyenv

指令	說明
```
pyenv install --list | grep -E '^\s*3\.[0-9]+\.[0-9]+$' | tail -n 10    列出目前 python 版本前10
pyenv versions	列出所有已安裝的版本
pyenv install 3.11.6	安裝指定版本的 Python
pyenv global 3.11.6	設定全域使用的 Python 版本（切換）
pyenv local 3.10.0	為目前資料夾設定特定版本（會產生 .python-version）——帶表不是全域 當下資料夾
pyenv uninstall 3.10.0	移除已安裝的版本
pyenv doctor	檢查安裝環境是否正常（需安裝插件）
```
### pip: Python 套件管理工具（用來安裝、更新、移除 Python 套件）
```
pip freeze	                     #顯示當前環境已安裝的所有套件（含版本）
pip list	                     #列出所有已安裝的套件
pip install requests==2.31.0	 #安裝指定版本
pip install requests	         #安裝 requests 套件
pip install -r requirements.txt	 #安裝一個需求檔中列出的所有套件
pip uninstall requests	         #移除套件
```
### 搭配範例：
#### 安裝指定 Python 版本
```
pyenv install 3.11.6
pyenv local 3.11.6
```
#### 建立虛擬環境
如果終端機有出現 venv 代表已啟動 
```
python -m venv venv
source venv/bin/activate
```
#### 安裝套件
`pip install requests flask`

### PythonCrawler 建立虛擬環境以及安裝 playwright
#### 建立虛擬環境（已建立過可跳過）
`python -m venv venv`
#### 啟動虛擬環境
MAC: `source venv/bin/activate`
#### 安裝套件（已安裝可跳過）
```
pip install playwright    
playwright install        #安裝 Playwright 瀏覽器核心（必須執行，Playwright 才能正常使用）
```

#### 腳本執行範例
```
python test_playwright.py
```
如果是 python3
```
python3 test_playwright.py
```

[playwright文件](https://playwright.dev/python/docs/intro)

