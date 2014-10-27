(ns tweet-searcher.core
  (:require [clj-http.client :as http]
			[clojure.data.json :as json]))

(def consumer_key "")
(def consumer_secret "")

(defn search [term]
	(let [query_response_body
		(let [bearer_token
			(let [bearer_token_response_body
				(:body (http/post "https://api.twitter.com/oauth2/token"
							{:basic-auth [consumer_key consumer_secret]
							 :headers {:content-type "application/x-www-form-urlencoded;charset=UTF-8"}
							 :form-params {:grant_type "client_credentials"}}))]
			(:access_token (json/read-str bearer_token_response_body :key-fn keyword)))]			
		(:body (http/get "https://api.twitter.com/1.1/search/tweets.json"
					{:headers {:authorization (str "Bearer " bearer_token)}
					 :query-params {"q" term, "count" "100"}})))]
		(map :text (:statuses (json/read-str query_response_body :key-fn keyword)))))	
  